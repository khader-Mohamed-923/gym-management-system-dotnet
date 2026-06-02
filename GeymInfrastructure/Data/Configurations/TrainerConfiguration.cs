using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Data.Configurations;

public class TrainerConfiguration : UserConfiguration<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);
        builder.Property(t => t.Speciality)
                .HasConversion<string>()
                .HasMaxLength(20);
    }
}
