using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Data.Configurations;

internal class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
{
    public void Configure(EntityTypeBuilder<MemberShip> builder)
    {
   


        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_MemberShip_DataRange", "StartDate < EndDate");

        });

        builder.HasIndex(m => new { m.MemberId, m.PlanId }).IsUnique();

        builder.HasQueryFilter(mb => !mb.IsDeleted);

    }
}
