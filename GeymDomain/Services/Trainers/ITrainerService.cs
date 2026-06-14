using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Trainers.Requests;
using GymManagement.Domain.DTOs.Trainers.Responses;

namespace GymManagement.Domain.Services.Trainers;

public interface ITrainerService
{
    Task<Result<IEnumerable<TrainerResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result> CreateAsync(CreateTrainerRequest request, CancellationToken cancellationToken);
    Task<TrainerDetailsResponse?> GetDetailsAsync(int id, CancellationToken cancellationToken);
    Task<TrainerEditResponse?> GetForEditAsync(int id, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(int id, UpdateTrainerRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}
