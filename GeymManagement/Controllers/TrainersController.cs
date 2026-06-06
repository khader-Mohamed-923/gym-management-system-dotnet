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
}
