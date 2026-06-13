using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Plans.Requests;
using GymManagement.Domain.DTOs.Plans.Responses;
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

        public async Task<IEnumerable<PlanResponse>> GetAllPlansAsync(CancellationToken cancellationToken)
        {
            var plans = await _planRepository.GetAllAsync(cancellationToken);

            return plans.Select(p => new PlanResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DurationInDays = p.DurationInDays,
                IsActive = p.IsActive
            }).ToList();
        }

        public async Task<PlanResponse?> GetPlanByIdAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);
            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null) return null;

            return new PlanResponse
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays,
                IsActive = plan.IsActive
            };
        }

        public async Task<PlanResponse?> GetPlanForEditAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);
            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null) return null;

            return new PlanResponse
            {
                Id = plan.Id,
                Name = plan.Name,
                DurationInDays = plan.DurationInDays,
                Price = plan.Price,
                Description = plan.Description,
                IsActive = plan.IsActive
            };
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanRequest request, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<Plan>(p => p.Id == id);
            var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);

            if (plan == null)
            {
                return Result.Failure("Plan not found.", nameof(id));
            }

            if (!string.Equals(plan.Name, request.PlanName, StringComparison.Ordinal))
            {
                return Result.Failure("Plan name cannot be modified.", nameof(request.PlanName));
            }

            var hasActiveMemberships = await HasActiveMembershipsAsync(id, cancellationToken);
            if (hasActiveMemberships)
            {
                return Result.Failure("Cannot update a plan that has active memberships.", nameof(id));
            }

            plan.DurationInDays = request.DurationInDays;
            plan.Price = request.Price;
            plan.Description = request.Description;

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
