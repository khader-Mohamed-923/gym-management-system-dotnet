using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Sessions;
using GymManagement.Domain.Services.Trainers;
using GymManagement.Domain.DTOs.Sessions.Requests;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Entities;
using GymManagement.Presentation.ViewModels.Session;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GymManagement.Presentation.Controllers;

public class SessionsController(
    ISessionService sessions,
    ITrainerService trainers,
    IMemberRepository<Category> categories) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await sessions.GetAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            var viewModels = result.Value.Adapt<IEnumerable<SessionIndexViewModel>>();
            return View(viewModels);
        }

        TempData["ErrorMessage"] = result.Error ?? "Failed to load sessions.";
        return View(new List<SessionIndexViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateDropdowns(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SessionCreateViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(cancellationToken);
            return View(viewModel);
        }

        var request = viewModel.Adapt<CreateSessionRequest>();

        var result = await sessions.CreateAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
            TempData["ErrorMessage"] = "Session Failed To Create. " + result.Error;
            await PopulateDropdowns(cancellationToken);
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Session Created Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var response = await sessions.GetDetailsAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Session not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<SessionDetailsViewModel>();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var response = await sessions.GetForEditAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Session not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<SessionEditViewModel>();
        await PopulateDropdowns(cancellationToken);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, SessionEditViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(cancellationToken);
            return View(viewModel);
        }

        var request = viewModel.Adapt<UpdateSessionRequest>();

        var result = await sessions.UpdateAsync(id, request, cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error!);
            TempData["ErrorMessage"] = "Cannot Update Session: " + result.Error;
            await PopulateDropdowns(cancellationToken);
            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Session Updated Successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await sessions.GetForEditAsync(id, cancellationToken);

        if (response == null)
        {
            TempData["ErrorMessage"] = "Session not found.";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = response.Adapt<SessionEditViewModel>();
        ViewBag.Id = viewModel.Id;
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var result = await sessions.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Session Deleted Successfully";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns(CancellationToken cancellationToken)
    {
        var trainersResult = await trainers.GetAllAsync(cancellationToken);
        var categoriesList = await categories.GetAllAsync(cancellationToken);

        ViewBag.Trainers = trainersResult.IsSuccess 
            ? trainersResult.Value.Select(t => new TrainerDropdownItem { Id = t.Id, Name = t.Name }).ToList() 
            : new List<TrainerDropdownItem>();

        ViewBag.Categories = categoriesList.Select(c => new CategoryDropdownItem { Id = c.Id, Name = c.Name }).ToList();
    }

    public record TrainerDropdownItem { public int Id { get; set; } public string Name { get; set; } = default!; }
    public record CategoryDropdownItem { public int Id { get; set; } public string Name { get; set; } = default!; }
}
