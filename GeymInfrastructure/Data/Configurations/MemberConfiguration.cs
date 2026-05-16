

using GymManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Data.Configurations;

public class MemberConfiguration : UserConfiguration<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);
        builder.Property(m => m.Photo)
            .HasMaxLength(500);
    }
}