using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Plans.Requests;
using GymManagement.Domain.DTOs.Plans.Responses;

namespace GymManagement.Domain.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanResponse>> GetAllPlansAsync(CancellationToken cancellationToken);
        Task<PlanResponse?> GetPlanByIdAsync(int id, CancellationToken cancellationToken);
        Task<PlanResponse?> GetPlanForEditAsync(int id, CancellationToken cancellationToken);
        Task<Result> UpdatePlanAsync(int id, UpdatePlanRequest request, CancellationToken cancellationToken);
        Task<Result> TogglePlanStatusAsync(int id, CancellationToken cancellationToken);
    }
}
