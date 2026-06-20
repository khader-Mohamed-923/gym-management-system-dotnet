using GymManagement.Domain.Enums;
namespace GymManagement.Presentation.ViewModels.Booking;

public class SessionMembersViewModel
{
    public int SessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SessionStatus Status { get; set; }
    public IEnumerable<SessionMemberViewModel> Members { get; set; } = [];
}

