using GymManagement.Presentation.Constants;
using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Bookings.Requests;
using GymManagement.Domain.Services.Bookings;
using GymManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Mapster;
using GymManagement.Presentation.ViewModels.Booking;

namespace GymManagement.Presentation.Controllers;

[Authorize]
public class BookingController : BaseController
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> Schedule(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        var upcomingResult = await _bookingService.GetUpcomingSessionsAsync(userId, cancellationToken);
        var ongoingResult = await _bookingService.GetOngoingSessionsAsync(userId, cancellationToken);

        var viewModel = new ScheduleViewModel
        {
            UpcomingSessions = upcomingResult.IsSuccess 
                ? upcomingResult.Value.Select(s => s.Adapt<SessionScheduleViewModel>()) 
                : Enumerable.Empty<SessionScheduleViewModel>(),
            OngoingSessions = ongoingResult.IsSuccess 
                ? ongoingResult.Value.Select(s => s.Adapt<SessionScheduleViewModel>()) 
                : Enumerable.Empty<SessionScheduleViewModel>()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Role.Member))]
    public async Task<IActionResult> Book(int sessionId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "User not authenticated." });
        }

        var result = await _bookingService.BookSessionAsync(sessionId, userId, cancellationToken);

        if (result.IsFailure)
        {
            return Json(new { success = false, message = result.Error });
        }

        return Json(new { success = true, message = "Session booked successfully!", bookingId = result.Value });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Member))]
    public async Task<IActionResult> MyBookings(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var result = await _bookingService.GetMemberBookingsAsync(userId, cancellationToken);

        if (result.IsFailure)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return View(Enumerable.Empty<BookingViewModel>());
        }

        var viewModels = result.Value.Select(b => b.Adapt<BookingViewModel>());

        return View(viewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Role.Member))]
    public async Task<IActionResult> Cancel(int bookingId, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "User not authenticated." });
        }

        var result = await _bookingService.CancelBookingAsync(bookingId, userId, cancellationToken);

        if (result.IsFailure)
        {
            return Json(new { success = false, message = result.Error });
        }

        return Json(new { success = true, message = "Booking cancelled successfully!" });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> SessionMembers(int sessionId, CancellationToken cancellationToken)
    {
        var result = await _bookingService.GetSessionMembersAsync(sessionId, cancellationToken);

        if (result.IsFailure)
        {
            TempData[TempDataKeys.ErrorMessage] = result.Error;
            return RedirectToAction(nameof(Schedule));
        }

        var viewModel = new SessionMembersViewModel
        {
            SessionId = result.Value.SessionId,
            SessionName = result.Value.SessionName,
            TrainerName = result.Value.TrainerName,
            StartDate = result.Value.StartDate,
            EndDate = result.Value.EndDate,
            Status = result.Value.Status,
            Members = result.Value.Members.Select(m => new SessionMemberViewModel
            {
                BookingId = m.BookingId,
                MemberId = m.MemberId,
                MemberName = m.MemberName,
                MemberEmail = m.MemberEmail,
                MemberPhone = m.MemberPhone,
                IsAttended = m.IsAttended,
                BookingDate = m.BookingDate
            })
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> MarkAttendance(int bookingId, bool attended, CancellationToken cancellationToken)
    {
        var result = await _bookingService.MarkAttendanceAsync(bookingId, attended, cancellationToken);

        if (result.IsFailure)
        {
            return Json(new { success = false, message = result.Error });
        }

        return Json(new { success = true, message = attended ? "Attendance marked." : "Attendance unmarked." });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Role.Trainer))]
    public async Task<IActionResult> MySessions(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var result = await _bookingService.GetTrainerSessionsAsync(userId, cancellationToken);

        var viewModel = new ScheduleViewModel
        {
            UpcomingSessions = result.IsSuccess 
                ? result.Value.Where(s => s.Status == SessionStatus.Upcoming).Select(s => s.Adapt<SessionScheduleViewModel>()) 
                : Enumerable.Empty<SessionScheduleViewModel>(),
            OngoingSessions = result.IsSuccess 
                ? result.Value.Where(s => s.Status == SessionStatus.Ongoing).Select(s => s.Adapt<SessionScheduleViewModel>()) 
                : Enumerable.Empty<SessionScheduleViewModel>()
        };

        return View("Schedule", viewModel);
    }


}





