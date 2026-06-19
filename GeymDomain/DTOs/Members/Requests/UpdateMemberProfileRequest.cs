namespace GymManagement.Domain.DTOs.Members.Requests;

public class UpdateMemberProfileRequest
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int BuildingNumber { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string BloodType { get; set; } = "Unknown";
    public string Notes { get; set; } = string.Empty;
}
