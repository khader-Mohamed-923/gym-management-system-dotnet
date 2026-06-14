namespace GymManagement.Domain.DTOs.Memberships.Responses;

public class MembershipResponse
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = default!;
    public int PlanId { get; set; }
    public string PlanName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
