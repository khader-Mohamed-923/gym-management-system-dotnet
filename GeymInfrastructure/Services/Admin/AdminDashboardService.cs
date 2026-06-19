using GymManagement.Domain.DTOs.Admin.Responses;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Services.Admin;

namespace GymManagement.Infrastructure.Services.Admin;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMembershipRepository _membershipRepository;

    public AdminDashboardService(
        IMemberRepository memberRepository,
        ITrainerRepository trainerRepository,
        ISessionRepository sessionRepository,
        IMembershipRepository membershipRepository)
    {
        _memberRepository = memberRepository;
        _trainerRepository = trainerRepository;
        _sessionRepository = sessionRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var members    = await _memberRepository.GetAllAsync(cancellationToken);
        var trainers   = await _trainerRepository.GetAllAsync(cancellationToken);
        var sessions   = await _sessionRepository.GetAllAsync(cancellationToken);
        var memberships = await _membershipRepository.GetAllAsync(cancellationToken);

        var now         = DateTime.UtcNow;
        var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);

        var weeklySessions = sessions.Count(s => s.StartDate >= startOfWeek && s.StartDate < startOfWeek.AddDays(7));

        var activeMemberships = memberships.Where(m => m.EndDate >= now).ToList();

        var monthlyRevenue = activeMemberships
            .Where(m => m.Plan != null)
            .Sum(m => m.Plan.Price);

        var recentRegistrations = members
            .OrderByDescending(m => m.JoinDate)
            .Take(5)
            .Select(m => new RecentRegistrationResponse
            {
                Name     = m.Name,
                Email    = m.Email,
                PlanName = activeMemberships
                    .FirstOrDefault(ms => ms.MemberId == m.Id)
                    ?.Plan?.Name ?? "No Plan",
                Date   = m.JoinDate.ToString("MMM dd, yyyy"),
                Status = activeMemberships.Any(ms => ms.MemberId == m.Id) ? "Active" : "Inactive"
            });

        return new AdminDashboardResponse
        {
            TotalActiveMembers  = activeMemberships.Select(ms => ms.MemberId).Distinct().Count(),
            TotalTrainers       = trainers.Count,
            WeeklySessions      = weeklySessions,
            MonthlyRevenue      = monthlyRevenue,
            RecentRegistrations = recentRegistrations
        };
    }
}
