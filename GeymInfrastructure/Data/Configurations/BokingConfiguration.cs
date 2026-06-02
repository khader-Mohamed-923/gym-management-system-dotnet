

using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Data.Configurations;

internal class BokingConfiguration : IEntityTypeConfiguration<Boking>
{
    public void Configure(EntityTypeBuilder<Boking> builder)
    {


        builder.Property(b => b.IsAttended)
                .HasDefaultValue(false);



        builder.HasIndex(x => new
        {
            x.MemberId,
            x.SessionId

        }).IsUnique();

        builder.HasQueryFilter(b => !b.IsDeleted);

        builder.HasOne(b => b.Member)
                .WithMany(s => s.Bokings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

    }
}
