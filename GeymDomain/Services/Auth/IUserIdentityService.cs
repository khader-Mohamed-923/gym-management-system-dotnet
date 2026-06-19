using GymManagement.Domain.Common;

namespace GymManagement.Domain.Services.Auth;

public interface IUserIdentityService
{
    Task<Result<string>> CreateUserAsync(string email, string password, string role, string firstName, string lastName);
}
