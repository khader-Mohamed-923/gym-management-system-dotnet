using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Trainer;

namespace GymManagement.Domain.Services.Trainers;

public interface ITrainerService
{
    Task<Result<IEnumerable<TrainerIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result> CreateAsync(TrainerCreateViewModel model, CancellationToken cancellationToken);
    Task<TrainerDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken);
    Task<TrainerEditViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(int id, TrainerEditViewModel model, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}
