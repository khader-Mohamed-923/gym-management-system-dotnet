using GymManagement.Domain.DTOs.Auth;
using GymManagement.Domain.Services.Auth;
using GymManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
public async Task<IActionResult> Register(RegisterRequest request)
{
    if (!ModelState.IsValid)
    {
        
        return View(request);
    }

    var result = await _authService.RegisterAsync(request);

    if (result.IsSuccess)
    {
        if (User.IsInRole(nameof(Role.Admin))) return RedirectToAction("Dashboard", "Admin");
        if (User.IsInRole(nameof(Role.Trainer))) return RedirectToAction("Dashboard", "Trainers");
        return RedirectToAction("Dashboard", "Member");
    }

    ModelState.AddModelError(string.Empty, result.Error ?? "Authentication failed");
    return View(request);
}

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var result = await _authService.LoginAsync(request);

        if (result.IsSuccess)
        {
            if (User.IsInRole(nameof(Role.Admin))) return RedirectToAction("Dashboard", "Admin");
            if (User.IsInRole(nameof(Role.Trainer))) return RedirectToAction("Dashboard", "Trainers");
            return RedirectToAction("Dashboard", "Member");
        }

        if (!string.IsNullOrEmpty(result.Error))
            ModelState.AddModelError(result.ErrorKey ?? string.Empty, result.Error);

        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}