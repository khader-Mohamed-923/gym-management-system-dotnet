using GymManagement.Domain.Services;
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
    }
}