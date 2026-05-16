using GeymInfrastructure;
using GeymManagement.DbContexts;
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
        // 1. تسجيل خدمات الـ Infrastructure كالعادة
        services.AddInfrastructure(configuration);

        // 2. كود الـ Seeding الأوتوماتيكي والاحترافي
        // بنبني ServiceProvider مؤقت عشان نعرف نطلع منه الـ DbContext
        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            try
            {
                // بنجيب الـ DbContext من الـ Scope
                var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();

                // بننادي على الـ Seeder بتاعك (وبنحولها لـ Synchronous عشان إحنا جوه ميثود static مش async)
                DatabaseSeeder.Seed(context).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // لو حصل أي Error في الـ Seeding، البرنامج مش هيقفل، وهيعدي عادي
                Console.WriteLine($"Seeding Failed: {ex.Message}");
            }
        }

        return services;
    }
}