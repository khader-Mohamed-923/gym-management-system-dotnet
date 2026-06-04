using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.Member;
using GymManagement.Infrastructure.Models;

namespace GymManagement.Domain.Services;

public interface IMemberService
{
    public Task<Result<IEnumerable<MemberIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken);
    public Task<Result> CreateAsync(CreateMemberViewModel model, CancellationToken cancellationToken);
}
