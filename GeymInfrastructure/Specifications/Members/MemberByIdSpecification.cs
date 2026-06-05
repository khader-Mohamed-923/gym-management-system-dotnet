using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Specifications;

namespace GymManagement.Infrastructure.Specifications.Members;

public class MemberByIdSpecification : BaseSpecification<Member>
{
    public MemberByIdSpecification(int id) : base(m => m.Id == id)
    {
        
        AddInclude(m => m.Address);
        AddInclude(m => m.Bookings);
        AddInclude(m => m.HealthRecord);
    }
}