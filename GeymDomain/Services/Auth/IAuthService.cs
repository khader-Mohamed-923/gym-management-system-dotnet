using GymManagement.Domain.Common;
using GymManagement.Domain.DTOs.Auth;

namespace GymManagement.Domain.Services.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}