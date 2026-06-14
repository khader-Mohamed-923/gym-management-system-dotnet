using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{


    public void Configure(EntityTypeBuilder<Session> builder)
    {


        builder.Property(s => s.Description)
            .HasMaxLength(200);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Session_Capacity", "Capacity BETWEEN 1 AND 25");
            t.HasCheckConstraint("CK_Session_DataRange", "StartDate < EndDate");

        });

        builder.HasQueryFilter(s => !s.IsDeleted);


    }
}
