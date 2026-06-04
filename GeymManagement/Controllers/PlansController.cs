using GeymInfrastructure.Repositories;
using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers;

public class PlansController(IRepository<Plan> plans) : Controller
{
  
    public async Task<IActionResult> Index()
    {
        var allplans = await plans.GetAllAsync();
        return View(allplans);
    }

    public async Task<IActionResult> Details(int id)
    {
        var plan = await plans.GetByIdAsync(id);
        if (plan is null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(plan);
    }
}
