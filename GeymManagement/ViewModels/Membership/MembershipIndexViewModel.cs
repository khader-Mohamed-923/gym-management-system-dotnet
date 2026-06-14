namespace GymManagement.Presentation.ViewModels.Membership;

public class MembershipIndexViewModel
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = default!;
    public int PlanId { get; set; }
    public string PlanName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = default!;
}
