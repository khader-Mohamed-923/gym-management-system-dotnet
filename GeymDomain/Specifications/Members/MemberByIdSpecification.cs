using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Specifications.Members;

public class MemberByIdSpecification : BaseSpecification<Member>
{
    public MemberByIdSpecification(int id) : base(m => m.Id == id)
    {
        
        AddInclude(m => m.Address);
        AddInclude(m => m.Bookings);
        AddInclude(m => m.HealthRecord);
    }
}
