using Autofac;
using Autofac.Extensions.DependencyInjection;
using GeymDomain;
using GymManagement.Infrastructure.IoC;
using GymManagement.Presentation.ExceptionHandlers;
using Serilog; // 1. ضيفنا الـ Using ده عشان يشوف Serilog

var builder = WebApplication.CreateBuilder(args);

// 2. بناء الـ Logger الأساسي وقراءة الإعدادات من الـ appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    Log.Information("Gym Management System is starting up... 🚀");

    // 3. بنقول للسيستم استخدم Serilog بدل الـ Logger بتاع دوت نت الافتراضي
    builder.Host.UseSerilog();

    // إعداد الـ Autofac بتاعك زي ما هو بالملي
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Services.AddDomain(builder.Configuration);
    builder.Services.AddControllersWithViews();
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new InfrastructureModule());
    });

    var app = builder.Build();

    app.UseExceptionHandler("/Home/Error");

    if (!app.Environment.IsDevelopment())
    {
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
    // لو حصلت أي قفلة والموقع بيقوم، يلقطها ويكتبها فوراً
    Log.Fatal(ex, "The Application failed to start correctly! ❌");
}
finally
{
    // تأمين قفل اللوجز وحفظها في الـ Seq والـ Console قبل ما السيرفر يقفل تماماً
    Log.CloseAndFlush();
}