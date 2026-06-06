using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Plan;

namespace GymManagement.Domain.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanIndexViewModel>> GetAllPlansAsync(CancellationToken cancellationToken);

        Task<PlanIndexViewModel?> GetPlanByIdAsync(int id, CancellationToken cancellationToken);

        Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken cancellationToken);

        Task<Result> UpdatePlanAsync(int id, EditPlanViewModel model, CancellationToken cancellationToken);

        Task<Result> TogglePlanStatusAsync(int id, CancellationToken cancellationToken);
    }
}