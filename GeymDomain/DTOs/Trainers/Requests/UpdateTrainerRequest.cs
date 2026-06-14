namespace GymManagement.Domain.DTOs.Trainers.Requests;

public class UpdateTrainerRequest
{
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public int BuildingNumber { get; set; }
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string Specialty { get; set; } = default!;
}
