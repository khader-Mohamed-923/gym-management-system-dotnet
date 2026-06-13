using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Specifications.Members;

public class MemberDetailsWithPlanSpecification : BaseSpecification<Member>
{
    public MemberDetailsWithPlanSpecification(int id) : base(m => m.Id == id)
    {
        AddInclude("MemberShips.Plan");

       
    }
}
