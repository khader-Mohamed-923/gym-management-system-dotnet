namespace GymManagement.Domain.DTOs.Sessions.Requests;

public class UpdateSessionRequest
{
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CategoryId { get; set; }
    public int TrainerId { get; set; }
}
