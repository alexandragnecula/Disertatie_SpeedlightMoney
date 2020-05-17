using System;
using DataLayer.Shared;

namespace DataLayer.Entities
{
    public class Friend : AuditableEntity
    { 
        public long Id { get; set; }
        public string Nickname { get; set; }
        public long UserId { get; set; }
        public long UserFriendId { get; set; }

        public ApplicationUser User { get; set; }
        public ApplicationUser UserFriend { get; set; }
    }
}
