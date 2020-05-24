using System;
namespace BusinessLayer.Views
{
    public class FriendDto
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public long UserId { get; set; }
        public long UserFriendId { get; set; }
    }
}
