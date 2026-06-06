using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Trainers;
using GymManagement.Domain.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers;

public class TrainersController(ITrainerService trainers) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await trainers.GetAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            return View(result.Value);
        }

        TempData["ErrorMessage"] = result.Error ?? "Failed to load trainers.";
        return View(new List<TrainerIndexViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainers.GetDetailsAsync(id, cancellationToken);

        if (trainer == null)
        {
            TempData["ErrorMessage"] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(trainer);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainerCreateViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await trainers.CreateAsync(model, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
            TempData["ErrorMessage"] = "Trainer Failed To Create. " + result.Error;
            return View(model);
        }

        TempData["SuccessMessage"] = "Trainer Created Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var trainer = await trainers.GetForEditAsync(id, cancellationToken);
        if (trainer == null)
        {
            TempData["ErrorMessage"] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(trainer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, TrainerEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await trainers.UpdateAsync(id, model, cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error!);
            TempData["ErrorMessage"] = "Cannot Update a Trainer: " + result.Error;
            return View(model);
        }

        TempData["SuccessMessage"] = "Trainer Updated Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var model = await trainers.GetForEditAsync(id, cancellationToken);

        if (model == null)
        {
            TempData["ErrorMessage"] = "Trainer not found.";
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
        var result = await trainers.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Trainer Deleted Successfully";
        return RedirectToAction(nameof(Index));
    }
}
