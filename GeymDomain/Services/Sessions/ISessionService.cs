using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Sessions.Requests;
using GymManagement.Domain.DTOs.Sessions.Responses;

namespace GymManagement.Domain.Services.Sessions;

public interface ISessionService
{
    Task<Result<IEnumerable<SessionResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result> CreateAsync(CreateSessionRequest request, CancellationToken cancellationToken);
    Task<SessionDetailsResponse?> GetDetailsAsync(int id, CancellationToken cancellationToken);
    Task<SessionEditResponse?> GetForEditAsync(int id, CancellationToken cancellationToken);
    Task<Result> UpdateAsync(int id, UpdateSessionRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
}
