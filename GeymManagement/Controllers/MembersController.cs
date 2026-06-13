using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Presentation.ViewModels.Member;
using GymManagement.Presentation.ViewModels.HealthRecord;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GymManagement.Presentation.Controllers;

public class MembersController(IMemberService members) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await members.GetAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            var viewModels = result.Value.Adapt<IEnumerable<MemberIndexViewModel>>();
            return View(viewModels);
        }

        TempData["ErrorMessage"] = result.Error ?? "Failed to load members.";
        return View(new List<MemberIndexViewModel>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MemberCreateViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var request = viewModel.Adapt<CreateMemberRequest>();

        var result = await members.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
            TempData["ErrorMessage"] = "Member Failed To Create. " + result.Error;
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Member Created Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var response = await members.GetDetailsAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<MemberDetailsViewModel>();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> HealthRecord(int id, CancellationToken cancellationToken)
    {
        var healthRecord = await members.GetHealthRecordAsync(id, cancellationToken);
        if (healthRecord == null)
        {
            TempData["ErrorMessage"] = "Health record not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = healthRecord.Adapt<HealthRecordDetailsViewModel>();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var response = await members.GetForEditAsync(id, cancellationToken);
        
        if (response == null)
        {
            TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<MemberEditViewModel>();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, MemberEditViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var request = viewModel.Adapt<UpdateMemberRequest>();

        var result = await members.UpdateAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error!);
            TempData["ErrorMessage"] = "Cannot Update a Member: " + result.Error;
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Member Updated Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await members.GetForEditAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<MemberEditViewModel>();
        ViewBag.id = viewModel.Id;
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await members.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Member Deleted Successfully";
        return RedirectToAction(nameof(Index));
    }
}
