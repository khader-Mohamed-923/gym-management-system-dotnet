using GymManagement.Domain.Enums;
using GymManagement.Domain.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers;

public class AdminController : BaseAdminController
{
    private readonly IAdminDashboardService _dashboardService;

    public AdminController(IAdminDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        var model = await _dashboardService.GetDashboardAsync(cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GenerateReport(CancellationToken cancellationToken)
    {
        var model = await _dashboardService.GetDashboardAsync(cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadReport(CancellationToken cancellationToken)
    {
        var fileBytes = await _dashboardService.GenerateReportCsvAsync(cancellationToken);
        return File(fileBytes, "text/csv", $"GymReport_{DateTime.Now:yyyyMMdd}.csv");
    }
}

