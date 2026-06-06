using GymManagement.Domain.Common;
using GymManagement.Domain.Services;
using GymManagement.Domain.ViewModels.Plan;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers
{
    public class PlansController(IPlanService planService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var viewModel = await planService.GetAllPlansAsync(cancellationToken);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var viewModel = await planService.GetPlanByIdAsync(id, cancellationToken);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var viewModel = await planService.GetPlanForEditAsync(id, cancellationToken);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPlanViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await planService.UpdatePlanAsync(id, model, cancellationToken);

            if (result.IsFailure)
            {
                ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);
                TempData["ErrorMessage"] = "Cannot update plan: " + result.Error;
                return View(model);
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