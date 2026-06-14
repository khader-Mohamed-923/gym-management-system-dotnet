using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Repositories;

public interface ISessionRepository : IMemberRepository<Session>
{
    Task<int> GetBookedCountAsync(int sessionId, CancellationToken cancellationToken = default);
    Task<bool> HasActiveBookingsAsync(int sessionId, CancellationToken cancellationToken = default);
}
