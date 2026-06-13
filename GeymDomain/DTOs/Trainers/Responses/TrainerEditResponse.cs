namespace GymManagement.Domain.DTOs.Trainers.Responses;

public class TrainerEditResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Specialty { get; set; } = default!;
    public int BuildingNumber { get; set; }
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
}
