using GymManagement.Domain.Enums;
using GymManagement.Domain.ValueObjects;


namespace GymManagement.Domain.Entities;

public class User :BaseEntity
{
    public string Name { get; set; }= string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateOnly  DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public Address Address { get; set; } = null!;

}
