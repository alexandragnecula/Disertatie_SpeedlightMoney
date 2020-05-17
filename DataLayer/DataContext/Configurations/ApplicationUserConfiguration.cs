using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("User");
            builder.Property(x => x.PhoneNumber)
                .IsRequired();
            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            builder.Property(x => x.FirstName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.LastName)
                .HasMaxLength(200);
            builder.Property(x => x.Email)
                .IsRequired();
            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.Property(x => x.CNP)
                .HasMaxLength(13)
                .IsRequired();
            builder.HasIndex(x => x.CNP)
                .IsUnique();
            builder.Property(x => x.Country)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.County)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.City)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.StreetName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.StreetNumber)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.CurrentStatus)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(x => x.CardNumber)
                .HasMaxLength(16)
                .IsRequired();
            builder.HasIndex(x => x.CardNumber)
                .IsUnique();
            builder.Property(x => x.Cvv)
               .HasMaxLength(3)
               .IsRequired();
        }
    }
}
