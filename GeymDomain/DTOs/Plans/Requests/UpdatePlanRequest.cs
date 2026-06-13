namespace GymManagement.Domain.DTOs.Plans.Requests;

public class UpdatePlanRequest
{
    public string PlanName { get; set; } = default!;
    public int DurationInDays { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
}
