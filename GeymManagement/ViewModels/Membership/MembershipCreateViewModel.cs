using System.ComponentModel.DataAnnotations;

namespace GymManagement.Presentation.ViewModels.Membership;

public class MembershipCreateViewModel
{
    [Required(ErrorMessage = "Member is required")]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "Plan is required")]
    public int PlanId { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime? StartDate { get; set; }
}
