namespace GymManagement.Domain.DTOs.Memberships.Requests;

public class CreateMembershipRequest
{
    public int MemberId { get; set; }
    public int PlanId { get; set; }
    public DateTime StartDate { get; set; }
}
