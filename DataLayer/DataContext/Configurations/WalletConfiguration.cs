using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class WalletConfiguration  : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable(nameof(Wallet));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(false);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Currency)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.CurrencyId);
        }
    }
}
