using GymManagement.Domain.Common;
using GymManagement.Domain.Services;
using GymManagement.Domain.ViewModels.Member;
using GymManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers;

public class MembersController(IMemberService members) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await members.GetAllAsync(cancellationToken);
        if (result.IsSuccess)
        {
            return View(result.Value);
        }
        return View(new List<MemberIndexViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await members.CreateAsync(model, cancellationToken);

        if (result.IsFailure)
        {
        
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
            return View(model);
        }

        TempData["SuccessMessage"] = "Member created successfully.";
        return RedirectToAction(nameof(Index));
    }
}