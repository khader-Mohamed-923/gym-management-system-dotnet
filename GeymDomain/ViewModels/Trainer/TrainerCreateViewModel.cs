using System.ComponentModel.DataAnnotations;

namespace GymManagement.Domain.ViewModels.Trainer;

public class TrainerCreateViewModel
{
    [Required(ErrorMessage = "Name Is Required")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Phone Number Is Required")]
    [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = default!;

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = default!;

    [Required(ErrorMessage = "Building Number Is Required")]
    [Range(1, 9000, ErrorMessage = "Building Number must be greater than 0")]
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
