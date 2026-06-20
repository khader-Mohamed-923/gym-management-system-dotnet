using GymManagement.Domain.Enums;
namespace GymManagement.Domain.DTOs.Bookings.Responses;

public class SessionScheduleResponse
{
    public int SessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DurationMinutes { get; set; }
    public int Capacity { get; set; }
    public int BookedCount { get; set; }
    public SessionStatus Status { get; set; }
    public bool IsBookedByCurrentUser { get; set; }
    public int? CurrentUserBookingId { get; set; }
}

