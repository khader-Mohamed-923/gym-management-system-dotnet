using GeymInfrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GeymManagement.Controllers;

public class PlansController : Controller
{
    private readonly IPlanRepository _planRepository;

    public PlansController(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task<IActionResult> Index()
    {
        var plans = await _planRepository.GetAllAsync();
        return View(plans);
    }

    public async Task<IActionResult> Details(int id)
    {
        var plan = await _planRepository.GetPlanById(id);
        if (plan is null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(plan);
    }
}
