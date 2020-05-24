using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class DebtStatusConfiguration : IEntityTypeConfiguration<DebtStatus>
    {
        public void Configure(EntityTypeBuilder<DebtStatus> builder)
        {
            builder.ToTable(nameof(DebtStatus));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(false);
            builder.Property(x => x.DebtStatusName)
                .IsRequired();
        }
    }
}
