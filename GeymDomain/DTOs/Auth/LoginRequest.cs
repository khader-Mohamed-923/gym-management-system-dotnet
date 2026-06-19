namespace GymManagement.Domain.DTOs.Auth;

public record LoginRequest(
    string UserName,
    string Password);