using GeymInfrastructure.Repositories;
using GeymInfrastructure.Data.Interceptors;
using GymManagement.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GymManagement.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using GymManagement.Infrastructure.Repositories;
using GymManagement.Infrastructure.Data.Identity;
using GymManagement.Domain.Services.Admin;
using GymManagement.Infrastructure.Services.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;


namespace GeymInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
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
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<GymManagement.Domain.Services.Auth.IAuthService, GymManagement.Infrastructure.Services.Auth.AuthService>();
        services.AddScoped<GymManagement.Domain.Services.Auth.IUserIdentityService, GymManagement.Infrastructure.Services.Auth.UserIdentityService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
        }   )
            .AddEntityFrameworkStores<GymDbContext>()
            .AddDefaultTokenProviders();

           services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly  = true;
            options.Cookie.SecurePolicy = env.IsDevelopment()
                ? CookieSecurePolicy.None
                : CookieSecurePolicy.Always;
            options.ExpireTimeSpan   = TimeSpan.FromDays(7);
            options.LoginPath        = "/Auth/Login";
            options.LogoutPath       = "/Auth/Logout";
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.SlidingExpiration = true;
        });
        return services;
    }
}
