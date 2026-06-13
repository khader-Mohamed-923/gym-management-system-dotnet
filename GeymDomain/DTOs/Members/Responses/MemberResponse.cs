namespace GymManagement.Domain.DTOs.Members.Responses;

public class MemberResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? PhotoPath { get; set; }
    public DateOnly JoinDate { get; set; }
    public string Gender { get; set; } = default!;
}
