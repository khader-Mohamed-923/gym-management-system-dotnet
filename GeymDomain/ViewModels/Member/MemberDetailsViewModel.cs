
namespace GymManagement.Domain.ViewModels.Member;

public class MemberDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; }= null!;

    public string? PhotoUrl { get; set; }


    public string Gender { get; set; }= null!;

    public string Address { get; set; } = null!;

    public string DateOfBirth { get; set; } = null!;
    public string PlanName { get; set; } = null!;

    public string MembershipStartDate { get; set; } = null!;

    public string MembershipEndDate { get; set;} = null!;


}
