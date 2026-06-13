using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(GymDbContext context)
    {
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Cardio" },
                new Category { Name = "Strength Training" },
                new Category { Name = "Flexibility" },
                new Category { Name = "Balance" },
                new Category { Name = "Endurance" }
            };
            await  context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
    }

}
