using GymManagement.Domain.DTOs.Auth;
using GymManagement.Domain.Services.Auth;
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
        return RedirectToAction("Dashboard", "Member");

    ModelState.AddModelError(string.Empty, result.Error);
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
            return RedirectToAction("Index", "Home");

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