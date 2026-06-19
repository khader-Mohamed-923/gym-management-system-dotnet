namespace GymManagement.Domain.DTOs.Members.Responses;

public class MemberMembershipResponse
{
    public int MembershipId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
