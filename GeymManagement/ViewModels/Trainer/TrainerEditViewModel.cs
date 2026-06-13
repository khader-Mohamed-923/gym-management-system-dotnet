using System.ComponentModel.DataAnnotations;

namespace GymManagement.Presentation.ViewModels.Trainer;

public class TrainerEditViewModel
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Phone Number Is Required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
    public string Phone { get; set; } = default!;

    [Required(ErrorMessage = "Building Number Is Required")]
    [Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than 0")]
    public int BuildingNumber { get; set; }

    [Required(ErrorMessage = "City Is Required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
    public string City { get; set; } = default!;

    [Required(ErrorMessage = "Street Is Required")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street can only contain letters, numbers, and spaces")]
    public string Street { get; set; } = default!;

    [Required(ErrorMessage = "Specialty is required")]
    public string Specialty { get; set; } = default!;
}
