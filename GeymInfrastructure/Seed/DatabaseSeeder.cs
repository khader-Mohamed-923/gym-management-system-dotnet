using GymManagement.Infrastructure.Data.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<GymDbContext>();
        
        await IdentitySeeder.SeedAsync(serviceProvider);
        await PlanSeeder.SeedAsync(context);
        await CategorySeeder.SeedAsync(context);
        await MemberSeeder.SeedAsync(context);
    }
}
