using GeymInfrastructure.Repositories;
using GymManagement.Domain.ViewModels.Plan;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;
using GymManagement.Infrastructure.Specifications;
namespace GymManagement.Domain.Services
{
    public class PlanService(IMemberRepository<Plan> planRepository) : IPlanService
    {
       

        public async Task<IEnumerable<PlanIndexViewModel>> GetAllPlansAsync(CancellationToken cancellationToken)
        {
            var plans = await planRepository.GetAllAsync(cancellationToken);

            return plans.Select(p => new PlanIndexViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DurationInDays = p.DurationInDays,
                IsActive = p.IsActive
            }).ToList();
        }




        public async Task<PlanIndexViewModel?> GetPlanByIdAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);

            var plan = await planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null) return null;

            return new PlanIndexViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays,
                IsActive = plan.IsActive
            };
        }
    }
}