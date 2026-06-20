namespace GymManagement.Presentation.ViewModels.Trainer;

public class TrainerDashboardViewModel
{
    public int TodaysSessionsCount { get; set; }
    public int TotalTraineesCount { get; set; }
    public double AverageRating { get; set; }
    public IEnumerable<TrainerSessionViewModel> UpcomingSessions { get; set; } = [];
}

public class TrainerSessionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string TimeRange { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int RegisteredTrainees { get; set; }
    public int MaxCapacity { get; set; }
}
