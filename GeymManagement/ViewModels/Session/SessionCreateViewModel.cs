using System.ComponentModel.DataAnnotations;

namespace GymManagement.Presentation.ViewModels.Session;

public class SessionCreateViewModel
{
    [Required(ErrorMessage = "Description is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 200 characters")]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Trainer is required")]
    public int TrainerId { get; set; }
}
