

namespace GymManagement.Domain.ViewModels.Member;

public class MemberIndexViewModel
{
    public int Id { get; set; }

    public string  Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string? Gender { get; set; }

    public string? PhotoPath { get; set; }

    public DateOnly JoinDate { get; set; } 




}
