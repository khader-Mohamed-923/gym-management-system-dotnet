using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Seed;

public static class PlanSeeder
{
    public static async Task SeedAsync(GymDbContext context)
    {
        if (!await context.Plans.AnyAsync())
        {
            var plans = new List<Plan>
        {
            new Plan { Name = "Basic Plan", Description = "Access to gym facilities during staffed hours.", Price = 29.99m, DurationInDays = 30 },
            new Plan { Name = "Premium Plan", Description = "24/7 access to gym facilities, plus free group classes.", Price = 49.99m, DurationInDays = 90 },
            new Plan { Name = "VIP Plan", Description = "All benefits of Premium Plan, plus personal training sessions and spa access.", Price = 79.99m, DurationInDays = 180 }
        };
            await context.Plans.AddRangeAsync(plans);
            await context.SaveChangesAsync();
        }
    }

}
