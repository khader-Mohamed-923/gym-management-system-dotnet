using GymManagement.Infrastructure.Models;
using GeymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GeymManagement.Data.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private static void UpdateAuditProperties(DbContext? context)
    {
        if (context == null)
            return;

        var gymContext = context as GymDbContext;
        var allowHardDelete = gymContext?.AllowHardDelete ?? false;

        var now = DateTime.UtcNow;

        var entries = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    entry.Entity.CreatedAt = now;
                    entry.Entity.IsDeleted = false;

                    break;

                case EntityState.Modified:

                    entry.Entity.UpdatedAt = now;

             
                    entry.Property(x => x.CreatedAt).IsModified = false;

                    break;

                case EntityState.Deleted:

                    if (allowHardDelete)
                    {
                        break;
                    }

                    entry.State = EntityState.Modified;

                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.UpdatedAt = now;

                    entry.Property(x => x.CreatedAt).IsModified = false;

                    break;
            }
        }
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditProperties(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditProperties(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}