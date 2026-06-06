namespace GymManagement.Domain.ViewModels.Trainer;

public class TrainerIndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Specialization { get; set; } = null!;
}
