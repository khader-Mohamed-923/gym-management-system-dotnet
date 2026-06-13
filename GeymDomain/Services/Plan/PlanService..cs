using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Plan;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Specifications;

namespace GymManagement.Domain.Services
{
    public class PlanService : IPlanService
    {
        private readonly IMemberRepository<Plan> _planRepository;
        private readonly IMemberRepository<MemberShip> _membershipRepository;

        public PlanService(
            IMemberRepository<Plan> planRepository,
            IMemberRepository<MemberShip> membershipRepository)
        {
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<IEnumerable<PlanIndexViewModel>> GetAllPlansAsync(CancellationToken cancellationToken)
        {
            var plans = await _planRepository.GetAllAsync(cancellationToken);

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

            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

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

        public async Task<EditPlanViewModel?> GetPlanForEditAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);
            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null) return null;

            return new EditPlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                DurationInDays = plan.DurationInDays,
                Price = plan.Price,
                Description = plan.Description
            };
        }

        public async Task<Result> UpdatePlanAsync(int id, EditPlanViewModel model, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);
            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null)
            {
                return Result.Failure("Plan not found.", nameof(id));
            }

            if (!string.Equals(plan.Name, model.PlanName, StringComparison.Ordinal))
            {
                return Result.Failure("Plan name cannot be modified.", nameof(model.PlanName));
            }

            var hasActiveMemberships = await HasActiveMembershipsAsync(id, cancellationToken);
            if (hasActiveMemberships)
            {
                return Result.Failure("Cannot update a plan that has active memberships.", nameof(id));
            }

            plan.DurationInDays = model.DurationInDays;
            plan.Price = model.Price;
            plan.Description = model.Description;

            await _planRepository.UpdateAsync(plan);
            await _planRepository.SaveChangesAsync();

            return Result.Success();
        }

        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<MemberShip>(m => m.PlanId == planId);
            var memberships = await _membershipRepository.GetListWithSpecAsync(spec, cancellationToken);

            return memberships.Any(m => m.EndDate >= DateTime.Today);
        }

        public async Task<Result> TogglePlanStatusAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);
            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null)
            {
                return Result.Failure("Plan not found.", nameof(id));
            }

            if (plan.IsActive)
            {
                var hasActiveMemberships = await HasActiveMembershipsAsync(id, cancellationToken);
                if (hasActiveMemberships)
                {
                    return Result.Failure("Cannot deactivate a plan with active memberships.", nameof(id));
                }
            }

            plan.IsActive = !plan.IsActive;

            await _planRepository.UpdateAsync(plan);
            await _planRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
