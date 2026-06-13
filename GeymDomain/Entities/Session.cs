namespace GymManagement.Domain.Entities;

public class Session : BaseEntity
{
    public string Description { get; set; } = null!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;


    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;


    public ICollection<Booking> Bokings { get; set; } = [];
}
