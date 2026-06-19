using GymManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeymManagement.Presentation.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole(nameof(Role.Admin)))
        {
            return RedirectToAction("Dashboard", "Admin");
        }
        else if (User.IsInRole(nameof(Role.Trainer)))
        {
            return RedirectToAction("Dashboard", "Trainer");
        }
        else if (User.IsInRole(nameof(Role.Member)))
        {
            return RedirectToAction("Dashboard", "Member");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}