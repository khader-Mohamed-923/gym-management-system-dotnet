using System.ComponentModel.DataAnnotations;

namespace GymManagement.Presentation.ViewModels.HealthRecord;

public class HealthRecordCreateViewModel
{
    [Required(ErrorMessage = "Height is required")]
    [Range(0.1, 300, ErrorMessage = "Height must be between 0.1 and 300 cm")]
    public decimal? Height { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(0.1, 500, ErrorMessage = "Weight must be between 0.1 and 500 kg")]
    public decimal? Weight { get; set; }

    [Required(ErrorMessage = "Blood Type Is Required")]
    [StringLength(10, ErrorMessage = "Invalid blood type")]
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; }
}
