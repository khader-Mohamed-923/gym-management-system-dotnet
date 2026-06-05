using GymManagement.Domain.ViewModels.Plan;

namespace GymManagement.Domain.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanIndexViewModel>> GetAllPlansAsync(CancellationToken cancellationToken);

        Task<PlanIndexViewModel?> GetPlanByIdAsync(int id, CancellationToken cancellationToken);
    }
}