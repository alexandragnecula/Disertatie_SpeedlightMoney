using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable(nameof(Currency));
            builder.Property(x => x.CurrencyName)
                .IsRequired();
            builder.HasIndex(x => x.CurrencyName)
                .IsUnique();
            builder.Property(x => x.Deleted)
               .HasDefaultValue(false);
        }
    }
}
