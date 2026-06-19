using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Bookings.Responses;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Specifications;
using GymManagement.Domain.Specifications.Members;

namespace GymManagement.Domain.Services.Bookings;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMemberRepository _memberRepository;

    public BookingService(
        IBookingRepository bookingRepository,
        ISessionRepository sessionRepository,
        IMemberRepository memberRepository)
    {
        _bookingRepository = bookingRepository;
        _sessionRepository = sessionRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<IEnumerable<SessionScheduleResponse>>> GetUpcomingSessionsAsync(string? userId, CancellationToken cancellationToken = default)
    {
        var spec = new SessionWithDetailsSpecification();
        var sessions = await _sessionRepository.GetListWithSpecAsync(spec, cancellationToken);
        var now = DateTime.Now;

        int? memberId = null;
        if (!string.IsNullOrEmpty(userId))
        {
            var memberSpec = new MemberByApplicationUserIdSpecification(userId);
            var member = await _memberRepository.GetEntityWithSpecAsync(memberSpec, cancellationToken);
            memberId = member?.Id;
        }

        var upcomingSessions = sessions
            .Where(s => s.StartDate > now)
            .OrderBy(s => s.StartDate)
            .ToList();

        var responses = new List<SessionScheduleResponse>();
        foreach (var session in upcomingSessions)
        {
            var bookedCount = await _sessionRepository.GetBookedCountAsync(session.Id, cancellationToken);
            var isBookedByCurrentUser = false;
            int? bookingId = null;

            if (memberId.HasValue)
            {
                isBookedByCurrentUser = await _bookingRepository.HasBookingAsync(memberId.Value, session.Id, cancellationToken);
                if (isBookedByCurrentUser)
                {
                    var booking = await _bookingRepository.GetMemberBookingAsync(memberId.Value, session.Id, cancellationToken);
                    bookingId = booking?.Id;
                }
            }

            responses.Add(new SessionScheduleResponse
            {
                SessionId = session.Id,
                SessionName = session.Description,
                CategoryName = session.Category?.Name ?? "Unknown",
                TrainerName = session.Trainer?.Name ?? "Unassigned",
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                DurationMinutes = (int)(session.EndDate - session.StartDate).TotalMinutes,
                Capacity = session.Capacity,
                BookedCount = bookedCount,
                Status = "Upcoming",
                IsBookedByCurrentUser = isBookedByCurrentUser,
                CurrentUserBookingId = bookingId
            });
        }

        return Result<IEnumerable<SessionScheduleResponse>>.Success(responses);
    }

    public async Task<Result<IEnumerable<SessionScheduleResponse>>> GetOngoingSessionsAsync(string? userId, CancellationToken cancellationToken = default)
    {
        var spec = new SessionWithDetailsSpecification();
        var sessions = await _sessionRepository.GetListWithSpecAsync(spec, cancellationToken);
        var now = DateTime.Now;

        int? memberId = null;
        if (!string.IsNullOrEmpty(userId))
        {
            var memberSpec = new MemberByApplicationUserIdSpecification(userId);
            var member = await _memberRepository.GetEntityWithSpecAsync(memberSpec, cancellationToken);
            memberId = member?.Id;
        }

        var ongoingSessions = sessions
            .Where(s => s.StartDate <= now && s.EndDate >= now)
            .OrderBy(s => s.EndDate)
            .ToList();

        var responses = new List<SessionScheduleResponse>();
        foreach (var session in ongoingSessions)
        {
            var bookedCount = await _sessionRepository.GetBookedCountAsync(session.Id, cancellationToken);
            var isBookedByCurrentUser = false;
            int? bookingId = null;

            if (memberId.HasValue)
            {
                isBookedByCurrentUser = await _bookingRepository.HasBookingAsync(memberId.Value, session.Id, cancellationToken);
                if (isBookedByCurrentUser)
                {
                    var booking = await _bookingRepository.GetMemberBookingAsync(memberId.Value, session.Id, cancellationToken);
                    bookingId = booking?.Id;
                }
            }

            responses.Add(new SessionScheduleResponse
            {
                SessionId = session.Id,
                SessionName = session.Description,
                CategoryName = session.Category?.Name ?? "Unknown",
                TrainerName = session.Trainer?.Name ?? "Unassigned",
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                DurationMinutes = (int)(session.EndDate - session.StartDate).TotalMinutes,
                Capacity = session.Capacity,
                BookedCount = bookedCount,
                Status = "Ongoing",
                IsBookedByCurrentUser = isBookedByCurrentUser,
                CurrentUserBookingId = bookingId
            });
        }

        return Result<IEnumerable<SessionScheduleResponse>>.Success(responses);
    }

    public async Task<Result<int>> BookSessionAsync(int sessionId, string memberUserId, CancellationToken cancellationToken = default)
    {
        // Get member
        var memberSpec = new MemberByApplicationUserIdSpecification(memberUserId);
        var member = await _memberRepository.GetEntityWithSpecAsync(memberSpec, cancellationToken);
        if (member == null)
        {
            return Result<int>.Failure("Member not found.", "Member");
        }

        // Check active membership
        var memberWithMembershipSpec = new MemberDetailsWithPlanSpecification(member.Id);
        var memberWithMembership = await _memberRepository.GetEntityWithSpecAsync(memberWithMembershipSpec, cancellationToken);
        
        var hasActiveMembership = memberWithMembership?.MemberShips?.Any(m => m.EndDate >= DateTime.Today) ?? false;
        if (!hasActiveMembership)
        {
            return Result<int>.Failure("You must have an active membership to book sessions.", "Membership");
        }

        // Get session
        var sessionSpec = new SessionWithDetailsSpecification(sessionId);
        var session = await _sessionRepository.GetEntityWithSpecAsync(sessionSpec, cancellationToken);
        if (session == null)
        {
            return Result<int>.Failure("Session not found.", "Session");
        }

        // Check session is in the future
        if (session.StartDate <= DateTime.Now)
        {
            return Result<int>.Failure("Cannot book sessions that have already started.", "Session");
        }

        // Check capacity
        var bookedCount = await _sessionRepository.GetBookedCountAsync(sessionId, cancellationToken);
        if (bookedCount >= session.Capacity)
        {
            return Result<int>.Failure("This session is fully booked.", "Session");
        }

        // Check if already booked or soft-deleted
        var existingBooking = await _bookingRepository.GetBookingIncludingDeletedAsync(member.Id, sessionId, cancellationToken);
        
        if (existingBooking != null)
        {
            if (!existingBooking.IsDeleted)
            {
                return Result<int>.Failure("You have already booked this session.", "Booking");
            }

            // Restore the soft-deleted booking instead of creating a new one
            existingBooking.IsDeleted = false;
            existingBooking.Date = DateTime.Now;
            existingBooking.IsAttended = false;
            
            await _bookingRepository.UpdateAsync(existingBooking);
            await _bookingRepository.SaveChangesAsync();
            
            return Result<int>.Success(existingBooking.Id);
        }

        // Create new booking
        var booking = new Booking
        {
            MemberId = member.Id,
            SessionId = sessionId,
            Date = DateTime.Now,
            IsAttended = false
        };

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync();

        return Result<int>.Success(booking.Id);
    }

    public async Task<Result> CancelBookingAsync(int bookingId, string memberUserId, CancellationToken cancellationToken = default)
    {
        var memberSpec = new MemberByApplicationUserIdSpecification(memberUserId);
        var member = await _memberRepository.GetEntityWithSpecAsync(memberSpec, cancellationToken);
        if (member == null)
        {
            return Result.Failure("Member not found.", "Member");
        }

        var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return Result.Failure("Booking not found.", "Booking");
        }

        if (booking.MemberId != member.Id)
        {
            return Result.Failure("You can only cancel your own bookings.", "Booking");
        }

        if (booking.Session.StartDate <= DateTime.Now)
        {
            return Result.Failure("Cannot cancel bookings for sessions that have already started.", "Session");
        }

        await _bookingRepository.SoftDeleteAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> MarkAttendanceAsync(int bookingId, bool attended, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId, cancellationToken);
        if (booking == null)
        {
            return Result.Failure("Booking not found.", "Booking");
        }

        var now = DateTime.Now;
        var session = booking.Session;
        
        // Allow attendance marking for ongoing or recently ended sessions (within 30 minutes)
        if (session.StartDate > now)
        {
            return Result.Failure("Cannot mark attendance for sessions that haven't started yet.", "Session");
        }

        // Allow marking attendance up to 30 minutes after session ends
        if (session.EndDate < now.AddMinutes(-30))
        {
            return Result.Failure("Cannot mark attendance for sessions that ended more than 30 minutes ago.", "Session");
        }

        booking.IsAttended = attended;
        await _bookingRepository.UpdateAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<IEnumerable<BookingResponse>>> GetMemberBookingsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var memberSpec = new MemberByApplicationUserIdSpecification(userId);
        var member = await _memberRepository.GetEntityWithSpecAsync(memberSpec, cancellationToken);
        if (member == null)
        {
            return Result<IEnumerable<BookingResponse>>.Failure("Member not found.", "Member");
        }

        var bookings = await _bookingRepository.GetListWithSpecAsync(
            new BookingWithDetailsSpecification(member.Id), cancellationToken);

        var now = DateTime.Now;
        var responses = bookings.Select(b => new BookingResponse
        {
            BookingId = b.Id,
            SessionId = b.SessionId,
            SessionName = b.Session?.Description ?? "Unknown",
            CategoryName = b.Session?.Category?.Name ?? "Unknown",
            TrainerName = b.Session?.Trainer?.Name ?? "Unassigned",
            StartDate = b.Session?.StartDate ?? DateTime.MinValue,
            EndDate = b.Session?.EndDate ?? DateTime.MinValue,
            DurationMinutes = b.Session != null ? (int)(b.Session.EndDate - b.Session.StartDate).TotalMinutes : 0,
            Capacity = b.Session?.Capacity ?? 0,
            BookedCount = 0, // Not needed for member view
            IsAttended = b.IsAttended,
            Status = b.Session != null 
                ? (b.Session.StartDate > now ? "Upcoming" : b.Session.EndDate < now ? "Completed" : "Ongoing")
                : "Unknown"
        }).OrderByDescending(b => b.Status == "Upcoming").ThenBy(b => b.StartDate);

        return Result<IEnumerable<BookingResponse>>.Success(responses);
    }

    public async Task<Result<SessionMembersDetailsResponse>> GetSessionMembersAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        var sessionSpec = new SessionWithDetailsSpecification(sessionId);
        var session = await _sessionRepository.GetEntityWithSpecAsync(sessionSpec, cancellationToken);
        if (session == null)
        {
            return Result<SessionMembersDetailsResponse>.Failure("Session not found.", "Session");
        }

        var bookings = await _bookingRepository.GetListWithSpecAsync(
            new BookingsBySessionSpecification(sessionId), cancellationToken);

        var now = DateTime.Now;
        var status = session.StartDate > now ? "Upcoming" : session.EndDate < now ? "Completed" : "Ongoing";

        var members = bookings.Select(b => new SessionMemberResponse
        {
            BookingId = b.Id,
            MemberId = b.MemberId,
            MemberName = b.Member?.Name ?? "Unknown",
            MemberEmail = b.Member?.Email ?? "N/A",
            MemberPhone = b.Member?.Phone ?? "N/A",
            IsAttended = b.IsAttended,
            BookingDate = b.Date
        });

        return Result<SessionMembersDetailsResponse>.Success(new SessionMembersDetailsResponse
        {
            SessionId = session.Id,
            SessionName = session.Description,
            TrainerName = session.Trainer?.Name ?? "Unassigned",
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            Status = status,
            Members = members
        });
    }
}

// Specifications for booking queries - placed in same file to avoid namespace issues
public class BookingWithDetailsSpecification : BaseSpecification<Booking>
{
    public BookingWithDetailsSpecification(int memberId) 
        : base(b => b.MemberId == memberId)
    {
        AddInclude(b => b.Session!);
        AddInclude("Session.Category");
        AddInclude("Session.Trainer");
    }
}

public class BookingsBySessionSpecification : BaseSpecification<Booking>
{
    public BookingsBySessionSpecification(int sessionId) 
        : base(b => b.SessionId == sessionId)
    {
        AddInclude(b => b.Member!);
    }
}
