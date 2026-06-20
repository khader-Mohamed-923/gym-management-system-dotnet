using GymManagement.Domain.Enums;
using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Bookings.Responses;

namespace GymManagement.Domain.Services.Bookings;

public interface IBookingService
{
    Task<Result<IEnumerable<SessionScheduleResponse>>> GetUpcomingSessionsAsync(string? userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SessionScheduleResponse>>> GetOngoingSessionsAsync(string? userId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SessionScheduleResponse>>> GetTrainerSessionsAsync(string trainerId, CancellationToken cancellationToken = default);
    Task<Result<int>> BookSessionAsync(int sessionId, string memberUserId, CancellationToken cancellationToken = default);
    Task<Result> CancelBookingAsync(int bookingId, string memberUserId, CancellationToken cancellationToken = default);
    Task<Result> MarkAttendanceAsync(int bookingId, bool attended, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BookingResponse>>> GetMemberBookingsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<SessionMembersDetailsResponse>> GetSessionMembersAsync(int sessionId, CancellationToken cancellationToken = default);
}

public class SessionMembersDetailsResponse
{
    public int SessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SessionStatus Status { get; set; }
    public IEnumerable<SessionMemberResponse> Members { get; set; } = [];
}

