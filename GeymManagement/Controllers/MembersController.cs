using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.ViewModels.Member;
using GymManagement.Infrastructure.Specifications;
using GymManagement.Infrastructure.Specifications.Members;
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


        TempData["ErrorMessage"] = result.Error ?? "Failed to load members.";
        return View(new List<MemberIndexViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MemberCreateViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await members.CreateAsync(model, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);

            TempData["ErrorMessage"] = "Member Failed To Create. " + result.Error;
            return View(model);
        }

        TempData["SuccessMessage"] = "Member Created Successfully";
        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var member = await members.GetDetailsAsync(id, cancellationToken);

        if (member == null)
        {
            TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(member);
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
        return View(healthRecord);
    }

    [HttpGet]

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var member = await members.GetForEditAsync(id, cancellationToken);
        if (member == null)
        {
            TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, MemberEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await members.UpdateAsync(id, model, cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error!);

            TempData["ErrorMessage"] = "Cannot Update a Member: " + result.Error;
            return View(model);
        }

        TempData["SuccessMessage"] = "Member Updated Successfully";
        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var model = await members.GetForEditAsync(id, cancellationToken);

        if (model == null)
        {
            TempData["ErrorMessage"] = "Member not found.";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.id = model.Id;
        return View(model);
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