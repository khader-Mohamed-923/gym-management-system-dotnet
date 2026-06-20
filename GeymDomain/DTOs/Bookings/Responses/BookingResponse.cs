using GymManagement.Domain.Enums;
namespace GymManagement.Domain.DTOs.Bookings.Responses;

public class BookingResponse
{
    public int BookingId { get; set; }
    public int SessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DurationMinutes { get; set; }
    public int Capacity { get; set; }
    public int BookedCount { get; set; }
    public bool IsAttended { get; set; }
    public SessionStatus Status { get; set; }
}

