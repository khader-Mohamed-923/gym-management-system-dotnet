using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Auth;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Services.Auth;
using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Services.Members;

namespace GymManagement.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly GymDbContext _context;
    private readonly IImageService _imageService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        GymDbContext context,
        IImageService imageService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _imageService = imageService;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        
        var existingUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUser != null)
            return Result<AuthResponse>.Failure("Username already exists.", nameof(request.UserName));

        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail != null)
            return Result<AuthResponse>.Failure("An account with this email already exists.", nameof(request.Email));

        var appUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.Phone
        };

        var result = await _userManager.CreateAsync(appUser, request.Password);
        if (!result.Succeeded)
            return Result<AuthResponse>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)), "Registration");

       
        string? photoUrl = null;
        if (request.ProfilePhotoFile != null)
        {
            using var stream = request.ProfilePhotoFile.OpenReadStream();
            var uploadResult = await _imageService.UploadAsync(stream, request.ProfilePhotoFile.FileName, "profiles");
            if (!uploadResult.IsSuccess)
            {
              
                await _userManager.DeleteAsync(appUser);
                return Result<AuthResponse>.Failure(uploadResult.Error, "Photo");
            }
            photoUrl = uploadResult.Value;
        }

        var member = new Member
        {
            Name = $"{request.FirstName} {request.LastName}",
            Email = request.Email,
            Phone = request.Phone ?? string.Empty,
            DateOfBirth = request.DateOfBirth ?? DateOnly.MinValue,
            Gender = request.Gender ?? Gender.Male,
            ApplicationUserId = appUser.Id,
            JoinDate = DateOnly.FromDateTime(DateTime.Now),
            Photo = photoUrl,
            Address = new GymManagement.Domain.ValueObjects.Address
            {
                Street = string.Empty,
                City = string.Empty,
                BuildingNumber = 0
            },
            HealthRecord = new HealthRecord
            {
                Height = 0,
                Weight = 0,
                BloodType = "Unknown",
                MedicalConditions = string.Empty
            }
        };

        _context.Users.Add(member);
        await _context.SaveChangesAsync();

        await _userManager.AddToRoleAsync(appUser, nameof(Role.Member));

        await _signInManager.SignInAsync(appUser, isPersistent: true);

        var roles = await _userManager.GetRolesAsync(appUser);

        return Result<AuthResponse>.Success(new AuthResponse(
            appUser.UserName!,
            appUser.Email!,
            roles.ToArray()));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var appUser = await _userManager.FindByNameAsync(request.UserName);
        if (appUser == null)
            return Result<AuthResponse>.Failure("Invalid username or password", "Login");

        var result = await _signInManager.PasswordSignInAsync(
            appUser, request.Password, isPersistent: true, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Result<AuthResponse>.Failure("Invalid username or password", "Login");

        var roles = await _userManager.GetRolesAsync(appUser);

        return Result<AuthResponse>.Success(new AuthResponse(
            appUser.UserName!,
            appUser.Email!,
            roles.ToArray()));
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
