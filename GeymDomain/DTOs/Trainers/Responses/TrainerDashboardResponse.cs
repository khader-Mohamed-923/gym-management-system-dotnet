namespace GymManagement.Domain.DTOs.Trainers.Responses;

public class TrainerDashboardResponse
{
    public int TodaysSessionsCount { get; set; }
    public int TotalTraineesCount { get; set; }
    public double AverageRating { get; set; }
    public IEnumerable<TrainerSessionResponse> UpcomingSessions { get; set; } = [];
}

public class TrainerSessionResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string TimeRange { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int RegisteredTrainees { get; set; }
    public int MaxCapacity { get; set; }
}
