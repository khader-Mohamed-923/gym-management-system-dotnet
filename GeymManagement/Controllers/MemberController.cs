using GymManagement.Domain.Enums;
using GymManagement.Infrastructure.Data.Identity;
using GymManagement.Domain.DTOs.Members.Responses;
using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Domain.Services.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers;

[Authorize(Roles = nameof(Role.Member))]
public class MemberController : Controller
{
    private readonly IMemberService _memberService;
    private readonly UserManager<ApplicationUser> _userManager;

    public MemberController(IMemberService memberService, UserManager<ApplicationUser> userManager)
    {
        _memberService = memberService;
        _userManager = userManager;
    }

    private async Task<string?> GetCurrentUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.Id;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return RedirectToAction("Login", "Auth");
        var profile = await _memberService.GetProfileAsync(userId);
        return View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> CompleteProfile()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return RedirectToAction("Login", "Auth");
        
        var profile = await _memberService.GetProfileAsync(userId);
        if (profile == null) return NotFound();

        var request = new UpdateMemberProfileRequest
        {
            Street = profile.Address?.Contains("Not provided") == false ? profile.Address.Split(',')[0].Trim() : "",
            City = profile.Address?.Contains("Not provided") == false && profile.Address.Contains(',') ? profile.Address.Split(',')[1].Trim() : "",
            Height = profile.HealthRecord?.Height ?? 0,
            Weight = profile.HealthRecord?.Weight ?? 0,
            BloodType = profile.HealthRecord?.BloodType ?? "Unknown",
            Notes = profile.HealthRecord?.Note ?? ""
        };

        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CompleteProfile(UpdateMemberProfileRequest request)
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(request);

        var result = await _memberService.UpdateProfileAsync(userId, request);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Profile completed successfully!";
            return RedirectToAction("Profile");
        }

        ModelState.AddModelError(string.Empty, result.Error ?? "An error occurred");
        return View(request);
    }

    public async Task<IActionResult> Bookings()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return RedirectToAction("Login", "Auth");
        var bookings = await _memberService.GetBookingsAsync(userId);
        return View(bookings);
    }

    public async Task<IActionResult> Memberships()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return RedirectToAction("Login", "Auth");
        var memberships = await _memberService.GetMembershipsAsync(userId);
        return View(memberships);
    }

    public async Task<IActionResult> Dashboard()
    {
        var userId = await GetCurrentUserIdAsync();
        if (userId == null) return RedirectToAction("Login", "Auth");

        var memberships = await _memberService.GetMembershipsAsync(userId);
        var bookings = await _memberService.GetBookingsAsync(userId);

        var model = new MemberDashboardResponse
        {
            ActiveMembership = memberships.FirstOrDefault(m => m.IsActive),
            TotalBookings = bookings.Count(),
            UpcomingBookings = bookings.Where(b => b.Status == "Upcoming").Take(2)
        };

        return View(model);
    }
}
