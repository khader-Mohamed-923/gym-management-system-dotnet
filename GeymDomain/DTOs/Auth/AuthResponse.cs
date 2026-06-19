namespace GymManagement.Domain.DTOs.Auth;

public record AuthResponse(
    string UserName,
    string Email,
    string[] Roles);