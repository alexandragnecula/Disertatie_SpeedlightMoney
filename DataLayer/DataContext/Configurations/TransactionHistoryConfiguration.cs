using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class TransactionHistoryConfiguration : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable(nameof(TransactionHistory));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(false);
            builder.HasOne(x => x.Sender)
                .WithMany(x => x.SenderTransactionsHistory)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Beneficiar)
               .WithMany(x => x.BeneficiarTransactionsHistory)
               .HasForeignKey(x => x.BeneficiarId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Currency)
               .WithMany(x => x.TransactionsHistory)
               .HasForeignKey(x => x.CurrencyId)
               .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
