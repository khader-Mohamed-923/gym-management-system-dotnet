namespace GymManagement.Domain.DTOs.Bookings.Responses;

public class AvailableMemberResponse
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public bool HasActiveMembership { get; set; }
}
