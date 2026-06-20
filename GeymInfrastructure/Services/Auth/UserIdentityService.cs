using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Auth;
using GymManagement.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace GymManagement.Infrastructure.Services.Auth;

public class UserIdentityService : IUserIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserIdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> CreateUserAsync(string email, string password, string role, string firstName, string lastName)
    {
        var existingEmail = await _userManager.FindByEmailAsync(email);
        if (existingEmail != null)
            return Result<string>.Failure("An account with this email already exists.", "Email");

        var appUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(appUser, password);
        if (!result.Succeeded)
            return Result<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)), "Registration");

        var roleResult = await _userManager.AddToRoleAsync(appUser, role);
        if (!roleResult.Succeeded)
            return Result<string>.Failure(string.Join(", ", roleResult.Errors.Select(e => e.Description)), "Role");

        return Result<string>.Success(appUser.Id);
    }

    public async Task<string?> GetCurrentUserIdAsync(System.Security.Claims.ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        return user?.Id;
    }
}
