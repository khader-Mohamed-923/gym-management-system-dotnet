using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Repositories;

public interface ITrainerRepository : IRepository<Trainer>
{
    Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<Trainer?> GetTrainerWithSessionsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
