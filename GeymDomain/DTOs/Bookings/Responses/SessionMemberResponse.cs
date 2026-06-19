namespace GymManagement.Domain.DTOs.Bookings.Responses;

public class SessionMemberResponse
{
    public int BookingId { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public string MemberPhone { get; set; } = string.Empty;
    public bool IsAttended { get; set; }
    public DateTime BookingDate { get; set; }
}
