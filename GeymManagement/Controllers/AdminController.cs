using GymManagement.Domain.Enums;
using GymManagement.Domain.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GymManagement.Presentation.Controllers;

[Authorize(Roles = nameof(Role.Admin))]
public class AdminController : Controller
{
    private readonly IAdminDashboardService _dashboardService;

    public AdminController(IAdminDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

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
        var model = await _dashboardService.GetDashboardAsync(cancellationToken);

        var csv = new StringBuilder();
        csv.AppendLine("GYM MANAGEMENT SYSTEM - REPORT");
        csv.AppendLine($"Generated:,{DateTime.Now:MMM dd yyyy  HH:mm}");
        csv.AppendLine();
        csv.AppendLine("SUMMARY");
        csv.AppendLine($"Active Members,{model.TotalActiveMembers}");
        csv.AppendLine($"Total Trainers,{model.TotalTrainers}");
        csv.AppendLine($"Weekly Sessions,{model.WeeklySessions}");
        csv.AppendLine($"Monthly Revenue,${model.MonthlyRevenue:N2}");
        csv.AppendLine();
        csv.AppendLine("RECENT REGISTRATIONS");
        csv.AppendLine("Name,Email,Plan,Join Date,Status");
        foreach (var reg in model.RecentRegistrations)
        {
            csv.AppendLine($"\"{reg.Name}\",\"{reg.Email}\",\"{reg.PlanName}\",\"{reg.Date}\",\"{reg.Status}\"");
        }

        var preamble = Encoding.UTF8.GetPreamble();
        var data = Encoding.UTF8.GetBytes(csv.ToString());
        var fileBytes = preamble.Concat(data).ToArray();

        return File(fileBytes, "text/csv", $"GymReport_{DateTime.Now:yyyyMMdd}.csv");
    }
}
