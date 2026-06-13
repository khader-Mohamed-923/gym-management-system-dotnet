using GymManagement.Domain.Common;
using GymManagement.Domain.Services;
using GymManagement.Domain.DTOs.Plans.Requests;
using GymManagement.Presentation.ViewModels.Plan;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace GymManagement.Presentation.Controllers
{
    public class PlansController(IPlanService planService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var responses = await planService.GetAllPlansAsync(cancellationToken);
            var viewModels = responses.Adapt<IEnumerable<PlanIndexViewModel>>();
            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var response = await planService.GetPlanByIdAsync(id, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            var viewModel = response.Adapt<PlanIndexViewModel>();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var response = await planService.GetPlanForEditAsync(id, cancellationToken);

            if (response == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = response.Adapt<EditPlanViewModel>();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPlanViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var request = viewModel.Adapt<UpdatePlanRequest>();

            var result = await planService.UpdatePlanAsync(id, request, cancellationToken);

            if (result.IsFailure)
            {
                ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
                TempData["ErrorMessage"] = "Cannot update plan: " + result.Error;
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Plan updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
        {
            var result = await planService.TogglePlanStatusAsync(id, cancellationToken);

            if (result.IsFailure)
            {
                TempData["ErrorMessage"] = result.Error;
            }
            else
            {
                TempData["SuccessMessage"] = "Plan status changed.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
