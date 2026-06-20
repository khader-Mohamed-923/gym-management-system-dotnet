namespace GymManagement.Presentation.ViewModels.Booking;

public class ScheduleViewModel
{
    public IEnumerable<SessionScheduleViewModel> UpcomingSessions { get; set; } = [];
    public IEnumerable<SessionScheduleViewModel> OngoingSessions { get; set; } = [];
}
