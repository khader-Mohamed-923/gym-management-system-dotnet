using Autofac;
using Autofac.Extensions.DependencyInjection;
using GeymDomain;
using GymManagement.Infrastructure.IoC;
using GymManagement.Presentation;
using Serilog;
using Microsoft.EntityFrameworkCore;
using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    Log.Information("Gym Management System is starting up");

    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new InfrastructureModule());
    });

    builder.Services.AddDomain(builder.Configuration);
    builder.Services.AddPresentation();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        await dbContext.Database.MigrateAsync();
        await DatabaseSeeder.Seed(dbContext);
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

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The Application failed to start correctly! ❌");
}
finally
{
    Log.CloseAndFlush();
}