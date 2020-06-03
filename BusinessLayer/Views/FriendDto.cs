using System;
using AutoMapper;
using BusinessLayer.Common.Mappings;
using DataLayer.Entities;

namespace BusinessLayer.Views
{
    public class FriendDto : IMapFrom<Friend>
    {
        public long Id { get; set; }
        public string Nickname { get; set; }

        //User
        public long UserId { get; set; }
        public string UserName { get; set; }

        //User Friend
        public long UserFriendId { get; set; }
        public string UserFriendName { get; set; }

        public bool Deleted { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Friend, FriendDto>()
                 .ForMember(d => d.UserName,
                    opt => opt.MapFrom(s =>
                        s.User != null
                            ? s.User.GetFullName()
                            : string.Empty))
                  .ForMember(d => d.UserFriendName,
                    opt => opt.MapFrom(s =>
                        s.UserFriend != null
                            ? s.UserFriend.GetFullName()
                            : string.Empty));
        }
    }
}
