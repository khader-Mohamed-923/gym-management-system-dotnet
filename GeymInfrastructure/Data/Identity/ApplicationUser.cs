using GymManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.Infrastructure.Data.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public User? Profile { get; set; }
}