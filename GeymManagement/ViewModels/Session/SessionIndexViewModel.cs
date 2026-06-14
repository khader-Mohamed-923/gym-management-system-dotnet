namespace GymManagement.Presentation.ViewModels.Session;

public class SessionIndexViewModel
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CategoryName { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public int TrainerId { get; set; }
    public int CategoryId { get; set; }
    public int BookedCount { get; set; }
    public string Status { get; set; } = default!;
}
