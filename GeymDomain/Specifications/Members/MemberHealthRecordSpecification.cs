using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Specifications.Members;

public class MemberHealthRecordSpecification : BaseSpecification<Member>
{
    public MemberHealthRecordSpecification(int memberId)
        : base(m => m.Id == memberId)
    {
        AddInclude(m => m.HealthRecord);
    }
}
