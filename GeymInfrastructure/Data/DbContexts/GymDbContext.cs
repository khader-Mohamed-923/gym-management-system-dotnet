using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Data.DbContexts;

public class GymDbContext : DbContext
{


    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }


    public bool AllowHardDelete { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
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


    public DbSet<Plan> Plans { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Member> Members => Set<Member>();

    public DbSet<Trainer> Trainers => Set<Trainer>();

    public DbSet<Session> Sessions { get; set; }

    public DbSet<Booking> Bokings { get; set; }

    public DbSet<MemberShip> MemberShips { get; set; }
}

