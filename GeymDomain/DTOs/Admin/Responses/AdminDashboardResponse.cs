using GymManagement.Domain.Enums;
namespace GymManagement.Domain.DTOs.Admin.Responses;

public class AdminDashboardResponse
{
    public int TotalActiveMembers { get; set; }
    public int TotalTrainers { get; set; }
    public int WeeklySessions { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public IEnumerable<RecentRegistrationResponse> RecentRegistrations { get; set; } = new List<RecentRegistrationResponse>();
}

public class RecentRegistrationResponse
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public MembershipStatus Status { get; set; }
}

