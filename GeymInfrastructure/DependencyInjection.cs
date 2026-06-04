using GeymInfrastructure.Repositories;
using GeymInfrastructure.Data.Interceptors;
using GeymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GymManagement.Infrastructure.Repositories;

namespace GeymInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddSingleton<AuditColumnsInterceptor>();

        services.AddDbContext<GymDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));


            options.AddInterceptors(
                sp.GetRequiredService<AuditColumnsInterceptor>());
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IMemberRepository, MemberRepository>();

        return services;
    }
}