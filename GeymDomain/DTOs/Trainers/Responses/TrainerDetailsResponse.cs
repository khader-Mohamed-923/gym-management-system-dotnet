namespace GymManagement.Domain.DTOs.Trainers.Responses;

public class TrainerDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string Specialization { get; set; } = default!;
    public string Address { get; set; } = default!;
}
