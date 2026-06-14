using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Repositories;

public interface IMembershipRepository : IMemberRepository<MemberShip>
{
    Task<bool> HasActiveMembershipAsync(int memberId, CancellationToken cancellationToken = default);
    Task<MemberShip?> GetActiveMembershipAsync(int memberId, CancellationToken cancellationToken = default);
}
