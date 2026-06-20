using GymManagement.Domain.DTOs.Admin.Responses;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Services.Admin;

using GymManagement.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Services.Admin;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly GymDbContext _dbContext;

    public AdminDashboardService(GymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        var weeklySessions = await _dbContext.Sessions
            .Where(s => s.StartDate >= startOfWeek && s.StartDate < endOfWeek && !s.IsDeleted)
            .CountAsync(cancellationToken);

        var activeMemberships = await _dbContext.MemberShips
            .Include(m => m.Plan)
            .Where(m => m.EndDate >= now && !m.IsDeleted)
            .ToListAsync(cancellationToken);

        var monthlyRevenue = activeMemberships
            .Where(m => m.Plan != null)
            .Sum(m => m.Plan.Price);

        var totalActiveMembers = activeMemberships
            .Select(m => m.MemberId)
            .Distinct()
            .Count();

        var totalTrainers = await _dbContext.Trainers
            .Where(t => !t.IsDeleted)
            .CountAsync(cancellationToken);

        var recentRegistrationsQuery = await _dbContext.Members
            .Where(m => !m.IsDeleted)
            .OrderByDescending(m => m.JoinDate)
            .Take(5)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Email,
                m.JoinDate,
                Membership = _dbContext.MemberShips
                    .Include(ms => ms.Plan)
                    .Where(ms => ms.MemberId == m.Id && ms.EndDate >= now && !ms.IsDeleted)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        var recentRegistrations = recentRegistrationsQuery.Select(m => new RecentRegistrationResponse
        {
            Name = m.Name,
            Email = m.Email,
            PlanName = m.Membership?.Plan?.Name ?? "No Plan",
            Date = m.JoinDate.ToString("MMM dd, yyyy"),
            Status = m.Membership != null ? GymManagement.Domain.Enums.MembershipStatus.Active : GymManagement.Domain.Enums.MembershipStatus.Inactive
        });

        return new AdminDashboardResponse
        {
            TotalActiveMembers = totalActiveMembers,
            TotalTrainers = totalTrainers,
            WeeklySessions = weeklySessions,
            MonthlyRevenue = monthlyRevenue,
            RecentRegistrations = recentRegistrations
        };
    }

    public async Task<byte[]> GenerateReportCsvAsync(CancellationToken cancellationToken = default)
    {
        var model = await GetDashboardAsync(cancellationToken);

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("GYM MANAGEMENT SYSTEM - REPORT");
        csv.AppendLine($"Generated:,{DateTime.Now:MMM dd yyyy  HH:mm}");
        csv.AppendLine();
        csv.AppendLine("SUMMARY");
        csv.AppendLine($"Active Members,{model.TotalActiveMembers}");
        csv.AppendLine($"Total Trainers,{model.TotalTrainers}");
        csv.AppendLine($"Weekly Sessions,{model.WeeklySessions}");
        csv.AppendLine($"Monthly Revenue,${model.MonthlyRevenue:N2}");
        csv.AppendLine();
        csv.AppendLine("RECENT REGISTRATIONS");
        csv.AppendLine("Name,Email,Plan,Join Date,Status");
        foreach (var reg in model.RecentRegistrations)
        {
            csv.AppendLine($"\"{reg.Name}\",\"{reg.Email}\",\"{reg.PlanName}\",\"{reg.Date}\",\"{reg.Status}\"");
        }

        var preamble = System.Text.Encoding.UTF8.GetPreamble();
        var data = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return preamble.Concat(data).ToArray();
    }
}
