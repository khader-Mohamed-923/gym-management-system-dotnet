using GymManagement.Presentation.Constants;
using GymManagement.Domain.Enums;
using GymManagement.Domain.DTOs.Members.Responses;
using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Presentation.Controllers;

[Authorize(Roles = nameof(Role.Member))]
public class MemberController : BaseController
{
    private readonly IMemberService _memberService;
    private readonly IUserIdentityService _userIdentityService;

    public MemberController(IMemberService memberService, IUserIdentityService userIdentityService)
    {
        _memberService = memberService;
        _userIdentityService = userIdentityService;
    }



    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return RedirectToAction("Login", "Auth");
        var profile = await _memberService.GetProfileAsync(userId);
        return View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> CompleteProfile()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return RedirectToAction("Login", "Auth");
        
        var profile = await _memberService.GetProfileAsync(userId);
        if (profile == null) return NotFound();

        var request = new UpdateMemberProfileRequest
        {
            Street = profile.Street,
            City = profile.City,
            BuildingNumber = profile.BuildingNumber,
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
        var userId = GetCurrentUserId();
        if (userId == null) return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
            return View(request);

        var result = await _memberService.UpdateProfileAsync(userId, request);

        if (result.IsSuccess)
        {
            TempData[TempDataKeys.SuccessMessage] = "Profile completed successfully!";
            return RedirectToAction("Profile");
        }

        ModelState.AddModelError(string.Empty, result.Error ?? "An error occurred");
        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Bookings()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return RedirectToAction("Login", "Auth");
        var bookings = await _memberService.GetBookingsAsync(userId);
        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> Memberships()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return RedirectToAction("Login", "Auth");
        var memberships = await _memberService.GetMembershipsAsync(userId);
        return View(memberships);
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return RedirectToAction("Login", "Auth");

        var result = await _memberService.GetMemberDashboardAsync(userId);
        if (result.IsFailure)
        {
            TempData[TempDataKeys.ErrorMessage] = "Could not load dashboard data.";
            return RedirectToAction("Login", "Auth");
        }

        return View(result.Value);
    }
}


