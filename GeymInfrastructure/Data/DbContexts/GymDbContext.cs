using GymManagement.Domain.Entities;
using GymManagement.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Data.DbContexts;

public class GymDbContext : IdentityDbContext<ApplicationUser>
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

    public bool AllowHardDelete { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Profile)
            .WithOne()
            .HasForeignKey<User>(u => u.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (!AllowHardDelete)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted)
                {
                    var isDeletedProp = entry.Entity.GetType().GetProperty("IsDeleted");
                    if (isDeletedProp != null)
                    {
                        entry.State = EntityState.Modified;
                        isDeletedProp.SetValue(entry.Entity, true);
                    }
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public new DbSet<User> Users { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<MemberShip> MemberShips { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
}
