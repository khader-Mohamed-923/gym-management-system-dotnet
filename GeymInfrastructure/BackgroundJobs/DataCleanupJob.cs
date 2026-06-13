
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GymManagement.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.BackgroundJobs;

public class DataCleanupJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DataCleanupJob> _logger;

    private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(30);

    public DataCleanupJob(IServiceScopeFactory scopeFactory, ILogger<DataCleanupJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Cleanup Job is starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting Hard Delete process for soft-deleted records...");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();

                    context.AllowHardDelete = true;

                    try
                    {
                        var deletedPlans = context.Plans
                             .IgnoreQueryFilters()
                             .Where(p => p.IsDeleted).ToList();
                        if (deletedPlans.Any())
                        {
                            context.Plans.RemoveRange(deletedPlans);
                            _logger.LogInformation("Removed {Count} soft-deleted Plans.", deletedPlans.Count);
                        }

                        var deletedCategories = context.Categories
                            .IgnoreQueryFilters()
                            .Where(c => c.IsDeleted).ToList();
                        if (deletedCategories.Any())
                        {
                            context.Categories.RemoveRange(deletedCategories);
                            _logger.LogInformation("Removed {Count} soft-deleted Categories.", deletedCategories.Count);
                        }

                        if (deletedPlans.Any() || deletedCategories.Any())
                        {
                            await context.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation("Database cleanup completed successfully.");
                        }
                        else
                        {
                            _logger.LogInformation("No soft-deleted records found to clean up.");
                        }
                    }
                    finally
                    {
                       
                        context.AllowHardDelete = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the data cleanup process.");
            }

            _logger.LogInformation("Data Cleanup Job is sleeping for {Days} days...", _cleanupInterval.TotalDays);

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}
