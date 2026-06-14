using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Domain.DTOs.Members.Responses;

namespace GymManagement.Domain.Services.Members;

public interface IMemberService
{
    Task<Result<IEnumerable<MemberResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result> CreateAsync(CreateMemberRequest request, CancellationToken cancellationToken);
    Task<MemberDetailsResponse?> GetDetailsAsync(int id, CancellationToken cancellationToken);
    Task<HealthRecordResponse?> GetHealthRecordAsync(int Id, CancellationToken cancellationToken);
    Task<MemberEditResponse?> GetForEditAsync(int id, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(int id, UpdateMemberRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}
