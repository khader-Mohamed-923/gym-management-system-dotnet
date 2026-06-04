using GymManagement.Presentation.ExceptionHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}
