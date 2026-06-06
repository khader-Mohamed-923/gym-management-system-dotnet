using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Trainer;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;

namespace GymManagement.Domain.Services.Trainers;

public class TrainerService(IMemberRepository<Trainer> trainerRepository) : ITrainerService
{
    public async Task<Result<IEnumerable<TrainerIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var trainers = await trainerRepository.GetAllAsync(cancellationToken);

        var viewModels = trainers.Select(t => new TrainerIndexViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            Specialization = t.Speciality.ToString()
        });

        return Result<IEnumerable<TrainerIndexViewModel>>.Success(viewModels);
    }
}
