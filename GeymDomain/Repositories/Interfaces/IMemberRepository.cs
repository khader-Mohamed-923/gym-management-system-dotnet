using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;

namespace GymManagement.Domain.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> HasUpcomingBookingsAsync(int id, DateTime dateTime, CancellationToken cancellationToken);
    Task<GymManagement.Domain.DTOs.Members.Responses.MemberDashboardResponse?> GetDashboardDataAsync(string applicationUserId, CancellationToken cancellationToken = default);
}
