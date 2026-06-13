using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Repositories;

public interface ITrainerRepository : IMemberRepository<Trainer>
{
    Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken cancellationToken = default);
}
