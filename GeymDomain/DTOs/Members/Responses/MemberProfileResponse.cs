namespace GymManagement.Domain.DTOs.Members.Responses;

public class MemberProfileResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string JoinDate { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public HealthRecordResponse HealthRecord { get; set; } = new();
}
