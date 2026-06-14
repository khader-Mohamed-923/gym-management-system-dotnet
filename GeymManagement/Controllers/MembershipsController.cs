using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Memberships;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.Services;
using GymManagement.Domain.DTOs.Memberships.Requests;
using GymManagement.Presentation.ViewModels.Membership;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GymManagement.Presentation.Controllers;

public class MembershipsController(
    IMembershipService memberships,
    IMemberService members,
    IPlanService plans) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await memberships.GetAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            var viewModels = result.Value.Adapt<IEnumerable<MembershipIndexViewModel>>();
            return View(viewModels);
        }

        TempData["ErrorMessage"] = result.Error ?? "Failed to load memberships.";
        return View(new List<MembershipIndexViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateDropdowns(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipCreateViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(cancellationToken);
            return View(viewModel);
        }

        var request = viewModel.Adapt<CreateMembershipRequest>();

        var result = await memberships.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
            TempData["ErrorMessage"] = "Membership Failed To Create. " + result.Error;
            await PopulateDropdowns(cancellationToken);
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Membership Created Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var result = await memberships.CancelAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Membership Cancelled Successfully";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns(CancellationToken cancellationToken)
    {
        var membersResult = await members.GetAllAsync(cancellationToken);
        var plansList = await plans.GetAllPlansAsync(cancellationToken);

        ViewBag.Members = membersResult.IsSuccess 
            ? membersResult.Value.Select(m => new MemberDropdownItem { Id = m.Id, Name = m.Name }).ToList() 
            : new List<MemberDropdownItem>();

        ViewBag.Plans = plansList.Where(p => p.IsActive)
                                 .Select(p => new PlanDropdownItem { Id = p.Id, Name = p.Name }).ToList();
    }

    public record MemberDropdownItem { public int Id { get; set; } public string Name { get; set; } = default!; }
    public record PlanDropdownItem { public int Id { get; set; } public string Name { get; set; } = default!; }
}
