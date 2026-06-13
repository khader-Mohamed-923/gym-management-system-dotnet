namespace GymManagement.Domain.DTOs.Members.Responses;

public class HealthRecordResponse
{
    public decimal Hight { get; set; }
    public decimal Weight { get; set; }
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; }
}
