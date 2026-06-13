using GymManagement.Domain.Entities;
using GymManagement.Domain.Specifications;

namespace GymManagement.Infrastructure.Specifications.Members;

public class MemberWithUpcomingBookingsSpecification : BaseSpecification<Booking>
{
    public MemberWithUpcomingBookingsSpecification(int id)
        : base(b => b.Id == id && b.Session!.EndDate >= DateTime.UtcNow)
    {
        AddInclude(b => b.Session!);
    }
}
