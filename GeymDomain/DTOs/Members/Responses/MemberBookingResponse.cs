using GymManagement.Domain.Enums;
namespace GymManagement.Domain.DTOs.Members.Responses;

public class MemberBookingResponse
{
    public int BookingId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public SessionStatus Status { get; set; }
}

