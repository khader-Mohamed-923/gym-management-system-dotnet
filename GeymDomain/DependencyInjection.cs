using GeymInfrastructure;
using GeymManagement.DbContexts;
using GymManagement.Domain.Services;
using GymManagement.Infrastructure.Seed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeymDomain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddScoped<IMemberService, MemberService>();


        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            try
            {
              
                var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();

                DatabaseSeeder.Seed(context).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed: {ex.Message}");
            }
        }

        return services;
    }
}