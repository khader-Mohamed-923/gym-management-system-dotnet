using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Memberships.Requests;
using GymManagement.Domain.DTOs.Memberships.Responses;

namespace GymManagement.Domain.Services.Memberships;

public interface IMembershipService
{
    Task<Result<IEnumerable<MembershipResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(CreateMembershipRequest request, CancellationToken cancellationToken = default);
    Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsMemberEligibleForBooking(int memberId, CancellationToken cancellationToken = default);
}
