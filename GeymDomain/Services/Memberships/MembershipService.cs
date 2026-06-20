using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Memberships.Requests;
using GymManagement.Domain.DTOs.Memberships.Responses;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Specifications;

namespace GymManagement.Domain.Services.Memberships;

public class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IRepository<Member> _memberRepository;
    private readonly IRepository<Plan> _planRepository;

    public MembershipService(
        IMembershipRepository membershipRepository,
        IRepository<Member> memberRepository,
        IRepository<Plan> planRepository)
    {
        _membershipRepository = membershipRepository;
        _memberRepository = memberRepository;
        _planRepository = planRepository;
    }

    public async Task<Result<IEnumerable<MembershipResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var memberships = await _membershipRepository.GetAllAsync(cancellationToken);
        
        var members = await _memberRepository.GetAllAsync(cancellationToken);
        var plans = await _planRepository.GetAllAsync(cancellationToken);

        var responses = memberships.Select(m => new MembershipResponse
        {
            Id = m.Id,
            MemberId = m.MemberId,
            MemberName = members.FirstOrDefault(x => x.Id == m.MemberId)?.Name ?? "Unknown",
            PlanId = m.PlanId,
            PlanName = plans.FirstOrDefault(x => x.Id == m.PlanId)?.Name ?? "Unknown",
            StartDate = m.StartDate,
            EndDate = m.EndDate
        });

        return Result<IEnumerable<MembershipResponse>>.Success(responses);
    }

    public async Task<Result> CreateAsync(CreateMembershipRequest request, CancellationToken cancellationToken = default)
    {

        var hasActive = await _membershipRepository.HasActiveMembershipAsync(request.MemberId, cancellationToken);
        if (hasActive)
        {
            return Result.Failure("A member can only have one active membership.", nameof(request.MemberId));
        }

        var spec = new BaseSpecification<Plan>(p => p.Id == request.PlanId);
        var plan = await _planRepository.GetEntityWithSpecAsync(spec, cancellationToken);
        
        if (plan == null)
        {
            return Result.Failure("Plan not found.", nameof(request.PlanId));
        }
        
        if (!plan.IsActive)
        {
            return Result.Failure("Only active plans can be assigned.", nameof(request.PlanId));
        }

        var membership = new MemberShip
        {
            MemberId = request.MemberId,
            PlanId = request.PlanId,
            StartDate = request.StartDate,
            EndDate = request.StartDate.AddDays(plan.DurationInDays),
            CreatedAt = DateTime.UtcNow
        };

        await _membershipRepository.AddAsync(membership, cancellationToken);
        await _membershipRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        var spec = new BaseSpecification<MemberShip>(m => m.Id == id);
        var membership = await _membershipRepository.GetEntityWithSpecAsync(spec, cancellationToken);

        if (membership == null)
        {
            return Result.Failure("Membership not found.", nameof(id));
        }

        await _membershipRepository.SoftDeleteAsync(membership, cancellationToken);
        await _membershipRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<bool> IsMemberEligibleForBooking(int memberId, CancellationToken cancellationToken = default)
    {
        return await _membershipRepository.HasActiveMembershipAsync(memberId, cancellationToken);
    }
}
