using GymManagement.Infrastructure.Models;

using Microsoft.EntityFrameworkCore;


namespace GeymManagement.DbContexts;

public class GymDbContext : DbContext
{
   

    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }


    public bool AllowHardDelete { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);


        modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Member>("Member")
                .HasValue<Trainer>("Trainer");


        modelBuilder.Entity<User>()
               .HasQueryFilter(b => !b.IsDeleted);


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

    public DbSet<Member> Members { get; set; }

    public DbSet<Trainer> Trainers { get; set; }

    public DbSet<Session> Sessions { get; set; }

    public DbSet<Boking> Bokings { get; set; }

    public DbSet<MemberShip> MemberShips { get; set; }
}
