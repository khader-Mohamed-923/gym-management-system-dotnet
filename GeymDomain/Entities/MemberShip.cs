namespace GymManagement.Domain.Entities;

public class MemberShip : BaseEntity
{

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;


    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
}
