using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
namespace BusinessLayer.Services.Friends
{
    public interface IFriendService
    {
        Task<Result> AddFriend(FriendDto friendToAdd);
        Task<Result> UpdateFriend(FriendDto friendToUpdate);
        Task<Result> DeleteFriend(FriendDto friendToDelete);
        Task<Result> RestoreFriend(FriendDto friendToRestore);
        Task<FriendDto> GetFriendById(long id);
        Task<IList<FriendDto>> GetAllFriends();
        Task<SelectItemVm> GetAllAsSelect(FriendDto friendDto);
    }
}
