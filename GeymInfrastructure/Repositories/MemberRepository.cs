using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeymInfrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    private readonly GymDbContext _dbContext;

    public MemberRepository(GymDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HasUpcomingBookingsAsync(int id, DateTime dateTime, CancellationToken cancellationToken)
    {
        return await _dbContext.Bookings 
            .AnyAsync(b => b.MemberId == id && b.Session != null && b.Session.EndDate >= dateTime, cancellationToken);
    }

    public async Task<bool> IsEmailTakenAsync(string normalizedEmail, int? excludeId = null, CancellationToken cancellationToken = default)
        => await _dbContext.Set<Member>().AnyAsync(m => m.Email == normalizedEmail && (!excludeId.HasValue || m.Id != excludeId.Value), cancellationToken);

    public async Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default)
        => await _dbContext.Set<Member>().AnyAsync(m => m.Phone == phone && (!excludeId.HasValue || m.Id != excludeId.Value), cancellationToken);

    public async Task<GymManagement.Domain.DTOs.Members.Responses.MemberDashboardResponse?> GetDashboardDataAsync(string applicationUserId, CancellationToken cancellationToken = default)
    {
        var member = await _dbContext.Set<Member>().FirstOrDefaultAsync(m => m.ApplicationUserId == applicationUserId, cancellationToken);
        if (member == null) return null;

        var today = DateTime.Today;
        var now = DateTime.Now;

        var activeMembership = await _dbContext.Set<MemberShip>()
            .Include(m => m.Plan)
            .Where(m => m.MemberId == member.Id && m.EndDate >= today)
            .OrderByDescending(m => m.EndDate)
            .Select(m => new GymManagement.Domain.DTOs.Members.Responses.MemberMembershipResponse
            {
                MembershipId = m.Id,
                PlanName = m.Plan != null ? m.Plan.Name : "Unknown Plan",
                Price = m.Plan != null ? m.Plan.Price : 0,
                StartDate = m.StartDate.ToShortDateString(),
                EndDate = m.EndDate.ToShortDateString(),
                IsActive = true
            })
            .FirstOrDefaultAsync(cancellationToken);

        var totalBookings = await _dbContext.Bookings.CountAsync(b => b.MemberId == member.Id, cancellationToken);

        var upcomingBookings = await _dbContext.Bookings
            .Include(b => b.Session)
            .ThenInclude(s => s.Trainer)
            .Where(b => b.MemberId == member.Id && b.Session != null && b.Session.StartDate >= now)
            .OrderBy(b => b.Session.StartDate)
            .Take(2)
            .Select(b => new GymManagement.Domain.DTOs.Members.Responses.MemberBookingResponse
            {
                BookingId = b.Id,
                SessionName = b.Session != null ? b.Session.Description : "Unknown",
                TrainerName = b.Session != null && b.Session.Trainer != null ? b.Session.Trainer.Name : "Unassigned",
                Date = b.Session != null ? b.Session.StartDate.ToShortDateString() : "N/A",
                Time = b.Session != null ? b.Session.StartDate.ToString("HH:mm") + " - " + b.Session.EndDate.ToString("HH:mm") : "N/A",
                Status = GymManagement.Domain.Enums.SessionStatus.Upcoming
            })
            .ToListAsync(cancellationToken);

        return new GymManagement.Domain.DTOs.Members.Responses.MemberDashboardResponse
        {
            ActiveMembership = activeMembership,
            TotalBookings = totalBookings,
            UpcomingBookings = upcomingBookings
        };
    }
}
