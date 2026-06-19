using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Specifications.Members;

public class MemberByApplicationUserIdSpecification : BaseSpecification<Member>
{
    public MemberByApplicationUserIdSpecification(string applicationUserId) 
        : base(m => m.ApplicationUserId == applicationUserId)
    {
        AddInclude(m => m.Address);
        AddInclude(m => m.HealthRecord);
        AddInclude("Bookings.Session.Trainer");
        AddInclude("MemberShips.Plan");
    }
}
