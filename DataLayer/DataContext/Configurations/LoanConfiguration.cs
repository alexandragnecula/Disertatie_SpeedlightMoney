using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable(nameof(Loan));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(true);
            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.HasOne(x => x.Borrower)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Lender)
                .WithMany(x => x.Borrows)
                .HasForeignKey(x => x.LenderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Currency)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.CurrencyId);
            builder.HasOne(x => x.LoanStatus)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.LoanStatusId);
            builder.HasOne(x => x.Term)
                .WithMany(x => x.Loans)
                .HasForeignKey(x => x.TermId);
        }
    }
}
