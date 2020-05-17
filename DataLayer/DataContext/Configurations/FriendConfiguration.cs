using System;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.DataContext.Configurations
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.ToTable(nameof(Friend));
            builder.Property(x => x.Deleted)
                 .HasDefaultValue(true);
            builder.Property(x => x.Nickname)
                .HasMaxLength(30);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserFriends)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.UserFriend)
               .WithMany(x => x.Users)
               .HasForeignKey(x => x.UserFriendId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
