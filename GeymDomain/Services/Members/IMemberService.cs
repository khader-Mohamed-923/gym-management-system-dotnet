using GymManagement.Domain.Common;
using GymManagement.Domain.ViewModels.HealthRecord;
using GymManagement.Domain.ViewModels.Member;

namespace GymManagement.Domain.Services.Members;

public interface IMemberService
{
    public Task<Result<IEnumerable<MemberIndexViewModel>>> GetAllAsync(CancellationToken cancellationToken);
    public Task<Result> CreateAsync(MemberCreateViewModel model, CancellationToken cancellationToken);

    public Task<MemberDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken);

    public Task<HealthRecordDetailsViewModel?> GetHealthRecordAsync(int Id, CancellationToken cancellationToken);


    public Task<MemberEditViewModel?> GetForEditAsync(int id, CancellationToken cancellationToken);


    public Task<Result> UpdateAsync(int id,MemberEditViewModel model, CancellationToken cancellationToken);

    public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}
