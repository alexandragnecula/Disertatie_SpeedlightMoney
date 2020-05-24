using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class DebtConfiguration : IEntityTypeConfiguration<Debt>
    {
        public void Configure(EntityTypeBuilder<Debt> builder)
        {
            builder.ToTable(nameof(Debt));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(false);

            builder.HasOne(x => x.Loan)
                .WithMany(x => x.Debts)
                .HasForeignKey(x => x.LoanId);
            builder.HasOne(x => x.DebtStatus)
                .WithMany(x => x.Debts)
                .HasForeignKey(x => x.DebtStatusId);
        }
    }
}
