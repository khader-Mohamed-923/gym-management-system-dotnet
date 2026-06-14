namespace GymManagement.Domain.DTOs.Sessions.Responses;

public class SessionEditResponse
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CategoryId { get; set; }
    public int TrainerId { get; set; }
}
