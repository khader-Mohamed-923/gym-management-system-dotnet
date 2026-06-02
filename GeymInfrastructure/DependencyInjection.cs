using GeymInfrastructure.Repositories;
using GeymManagement.Data.Interceptors;
using GeymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeymInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped<AuditSaveChangesInterceptor>();


        services.AddDbContext<GymDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));

            options.AddInterceptors(
                sp.GetRequiredService<AuditSaveChangesInterceptor>());
        });

   

        return services;
    }
}