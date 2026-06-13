using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Trainers;
using GymManagement.Domain.DTOs.Trainers.Requests;
using GymManagement.Presentation.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GymManagement.Presentation.Controllers;

public class TrainersController(ITrainerService trainers) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await trainers.GetAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            var viewModels = result.Value.Adapt<IEnumerable<TrainerIndexViewModel>>();
            return View(viewModels);
        }

        TempData["ErrorMessage"] = result.Error ?? "Failed to load trainers.";
        return View(new List<TrainerIndexViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var response = await trainers.GetDetailsAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<TrainerDetailsViewModel>();
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainerCreateViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var request = viewModel.Adapt<CreateTrainerRequest>();

        var result = await trainers.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
            TempData["ErrorMessage"] = "Trainer Failed To Create. " + result.Error;
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Trainer Created Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var response = await trainers.GetForEditAsync(id, cancellationToken);
        if (response == null)
        {
            TempData["ErrorMessage"] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<TrainerEditViewModel>();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, TrainerEditViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var request = viewModel.Adapt<UpdateTrainerRequest>();

        var result = await trainers.UpdateAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error!);
            TempData["ErrorMessage"] = "Cannot Update a Trainer: " + result.Error;
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Trainer Updated Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await trainers.GetForEditAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<TrainerEditViewModel>();
        ViewBag.id = viewModel.Id;
        return View(viewModel);
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
