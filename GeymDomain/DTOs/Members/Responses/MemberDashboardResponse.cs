namespace GymManagement.Domain.DTOs.Members.Responses;

public class MemberDashboardResponse
{
    public MemberMembershipResponse? ActiveMembership { get; set; }
    public int TotalBookings { get; set; }
    public IEnumerable<MemberBookingResponse> UpcomingBookings { get; set; } = new List<MemberBookingResponse>();
}
