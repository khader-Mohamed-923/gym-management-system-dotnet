using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;

namespace GeymInfrastructure.Repositories;

public interface IMemberRepository : IMemberRepository<Member>
{
    Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> HasUpcomingBookingsAsync(int id, DateTime dateTime, CancellationToken cancellationToken);
}
