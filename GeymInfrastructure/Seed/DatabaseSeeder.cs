using GymManagement.Infrastructure.Data.DbContexts;

namespace GymManagement.Infrastructure.Seed;

public static class DatabaseSeeder
{

    public static async Task Seed(GymDbContext context)
    {
        await PlanSeeder.SeedAsync(context);
        await CategorySeeder.SeedAsync(context);

        await MemberSeeder.SeedAsync(context);


    }
}

