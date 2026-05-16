

using GymManagement.Infrastructure.Models;
using GymManagement.Infrastructure.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Data.Configurations;

public class UserConfiguration<T> : IEntityTypeConfiguration<T> where T : User
{
  
   

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(u => u.Name)
            .HasMaxLength(40);

        builder.Property(u => u.Email)
            .HasMaxLength(100);

        builder.Property(u => u.Phone)
            .HasMaxLength(20);

        // Address as Owned Entity (Value Object)
        builder.OwnsOne(u => u.Address, a =>
        {
            a.Property(ad => ad.Street)
                .HasColumnName(nameof(Address.Street))
                .HasMaxLength(100);

            a.Property(ad => ad.City)
                .HasColumnName(nameof(Address.City))
                .HasMaxLength(100);

            a.Property(ad => ad.BuildingNumber)
                .HasColumnName(nameof(Address.BuildingNumber));
        });

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Phone)
            .IsUnique();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_User_Role", "LEN(Phone)=11 AND Phone LIKE '01[0125]%'");
        });
    }
}
