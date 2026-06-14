using System.ComponentModel.DataAnnotations;

namespace GymManagement.Presentation.ViewModels.Plan;

public class EditPlanViewModel
{
    public int Id { get; set; }

    public string PlanName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Duration is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than zero.")]
    [Display(Name = "Duration (Days)")]
    public int DurationInDays { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    [Display(Name = "Price (EGP)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
    public string Description { get; set; } = string.Empty;
}
