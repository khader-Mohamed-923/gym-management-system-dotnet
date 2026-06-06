using Autofac;
using Autofac.Extensions.DependencyInjection;
using GeymDomain;
using GymManagement.Infrastructure.IoC;
using GymManagement.Presentation;
using GymManagement.Presentation.Extensions; 
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    Log.Information("Gym Management System is starting up ");

    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new InfrastructureModule());
    });

    builder.Services.AddDomain(builder.Configuration);
    builder.Services.AddPresentation();

    var app = builder.Build();

    await app.ApplyMigrationsAndSeedAsync();

    app.ConfigureRequestPipeline();

    Log.Information("Gym Management System is running successfully! ");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The Application failed to start correctly!");
}
finally
{
    Log.CloseAndFlush();
}