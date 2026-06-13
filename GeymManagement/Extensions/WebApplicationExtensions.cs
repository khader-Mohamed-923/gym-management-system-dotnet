using GymManagement.Infrastructure.Data.DbContexts;
using GymManagement.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GymManagement.Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

        try
        {
            Log.Information("Applying Database Migrations... ⏳");
            await dbContext.Database.MigrateAsync();

            Log.Information("Seeding Database Data... 🌱");
            await DatabaseSeeder.Seed(dbContext);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating or seeding the database! ❌");
            throw;
        }
    }

    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        return app;
    }
}