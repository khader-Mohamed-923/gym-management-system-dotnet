using GymManagement.Domain.DTOs.Admin.Responses;

namespace GymManagement.Domain.Services.Admin;

public interface IAdminDashboardService
{
    Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task<byte[]> GenerateReportCsvAsync(CancellationToken cancellationToken = default);
}
