using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Trainer;

namespace GymManagement.Domain.Services.Trainers;

public interface ITrainerService
{
    Task<Result<IEnumerable<TrainerIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken);
}
