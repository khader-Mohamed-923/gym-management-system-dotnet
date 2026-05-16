
using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeymManagement.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(n => n.Name)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);


        builder.Property(n => n.Description)
                .HasMaxLength(200);


        builder.Property(n => n.Price)
               .HasPrecision(10, 2);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("planDurationCheck", "DurationInDays BETWEEN 1 AND 365 ");
        });



        builder.HasQueryFilter(p => !p.IsDeleted);



    }
}
