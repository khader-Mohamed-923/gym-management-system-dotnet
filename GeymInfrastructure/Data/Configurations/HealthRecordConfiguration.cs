using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymManagement.Infrastructure.Data.Configurations;

internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {


        builder.Property(hr => hr.Hight)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(hr => hr.Weight)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(hr => hr.BloodType)
            .HasConversion<string>()
            .HasMaxLength(40);


        builder.HasQueryFilter(h => !h.IsDeleted);

    }
}
