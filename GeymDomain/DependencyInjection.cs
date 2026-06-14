using GymManagement.Domain.Services;
using GymManagement.Domain.Services.Members;
using GymManagement.Domain.Services.Trainers;
using GymManagement.Domain.Services.Sessions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeymDomain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<ISessionService, SessionService>();

        return services;
    }
}
