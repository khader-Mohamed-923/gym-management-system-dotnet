namespace GymManagement.Presentation.ViewModels.HealthRecord;

public class HealthRecordDetailsViewModel
{

    public decimal? Hight { get; set; }

    public decimal? Weight { get; set; }

    public string BloodType { get; set; } = null!;
    public string? Note { get; set; } 
}
