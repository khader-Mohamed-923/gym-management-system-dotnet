namespace GymManagement.Presentation.ViewModels.Session;

public class SessionDetailsViewModel
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CategoryName { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public string TrainerEmail { get; set; } = default!;
    public string TrainerPhone { get; set; } = default!;
    public string TrainerSpecialization { get; set; } = default!;
    public int BookedCount { get; set; }
    public string Status { get; set; } = default!;
}
