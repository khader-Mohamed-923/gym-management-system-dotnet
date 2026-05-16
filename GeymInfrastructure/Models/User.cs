using GymManagement.Infrastructure.Enums;
using GymManagement.Infrastructure.ValueObjects;


namespace GymManagement.Infrastructure.Models;

public class User :BaseEntity
{
    public string Name { get; set; }= string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateOnly  DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public Address Address { get; set; } = null!;

}
