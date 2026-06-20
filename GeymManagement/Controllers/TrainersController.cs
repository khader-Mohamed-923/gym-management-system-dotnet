using GymManagement.Presentation.Constants;
using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Trainers;
using GymManagement.Domain.DTOs.Trainers.Requests;
using GymManagement.Presentation.ViewModels.Trainer;
using GymManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GymManagement.Presentation.Controllers;

public class TrainersController(ITrainerService trainers) : BaseController
{
    [HttpGet]
    [Authorize(Roles = nameof(Role.Trainer))]
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await trainers.GetAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            var viewModels = result.Value.Adapt<IEnumerable<TrainerIndexViewModel>>();
            return View(viewModels);
        }

        TempData[TempDataKeys.ErrorMessage] = result.Error ?? "Failed to load trainers.";
        return View(new List<TrainerIndexViewModel>());
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var response = await trainers.GetDetailsAsync(id, cancellationToken);

        if (response == null)
        {
            TempData[TempDataKeys.ErrorMessage] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<TrainerDetailsViewModel>();
        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Role.Admin))]
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
            TempData[TempDataKeys.ErrorMessage] = "Trainer Failed To Create. " + result.Error;
            return View(viewModel);
        }

        TempData[TempDataKeys.SuccessMessage] = "Trainer Created Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var response = await trainers.GetForEditAsync(id, cancellationToken);
        if (response == null)
        {
            TempData[TempDataKeys.ErrorMessage] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<TrainerEditViewModel>();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Role.Admin))]
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
            TempData[TempDataKeys.ErrorMessage] = "Cannot Update a Trainer: " + result.Error;
            return View(viewModel);
        }

        TempData[TempDataKeys.SuccessMessage] = "Trainer Updated Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await trainers.GetForEditAsync(id, cancellationToken);

        if (response == null)
        {
            TempData[TempDataKeys.ErrorMessage] = "Trainer not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<TrainerEditViewModel>();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await trainers.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        TempData[TempDataKeys.SuccessMessage] = "Trainer Deleted Successfully";
        return RedirectToAction(nameof(Index));
    }
}
