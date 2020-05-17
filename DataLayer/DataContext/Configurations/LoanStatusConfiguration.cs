using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class LoanStatusConfiguration : IEntityTypeConfiguration<LoanStatus>
    {
        public void Configure(EntityTypeBuilder<LoanStatus> builder)
        {
            builder.ToTable(nameof(LoanStatus));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(true);
            builder.Property(x => x.LoanStatusName)
                .IsRequired();
        }
    }
}
