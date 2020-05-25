using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Friends
{
    public class FriendService :IFriendService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public FriendService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> AddFriend(FriendDto friendToAdd)
        {
            var entity = new Friend
            {
                Nickname = friendToAdd.Nickname,
                UserId = friendToAdd.UserId,
                UserFriendId = friendToAdd.UserFriendId
            };

            await _context.Friends.AddAsync(entity);

            await _context.SaveChangesAsync();

            return Result.Success("Friend was created successfully");
        }

        public async Task<Result> UpdateFriend(FriendDto friendToUpdate)
        {
            var entity = await _context.Friends
               .FirstOrDefaultAsync(x => x.Id == friendToUpdate.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid friend found" });
            }

            entity.Nickname = friendToUpdate.Nickname;
            entity.UserId = friendToUpdate.UserId;
            entity.UserFriendId = friendToUpdate.UserFriendId;

            await _context.SaveChangesAsync();

            return Result.Success("Friend update was successful");
        }

        public async Task<Result> DeleteFriend(FriendDto friendToDelete)
        {
            var entity = await _context.Friends
               .FirstOrDefaultAsync(x => x.Id == friendToDelete.Id && !x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid friend found" });
            }

            entity.Deleted = true;

            await _context.SaveChangesAsync();

            return Result.Success("Friend was deleted");
        }

        public async Task<Result> RestoreFriend(FriendDto friendToRestore)
        {
            var entity = await _context.Friends
            .FirstOrDefaultAsync(x => x.Id == friendToRestore.Id && x.Deleted);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid friend found" });
            }

            entity.Deleted = false;
            entity.DeletedBy = null;
            entity.DeletedOn = null;

            await _context.SaveChangesAsync();

            return Result.Success("Friend was restored");
        }

        public async Task<FriendDto> GetFriendById(long id)
        {
            var entity = await _context.Friends.FindAsync(id);

            return entity == null ? null : _mapper.Map<FriendDto>(entity);

        }

        public async Task<IList<FriendDto>> GetAllFriends()
        {
            List<FriendDto> friends = await _context.Friends
               .OrderByDescending(x => x.CreatedOn)
               .ProjectTo<FriendDto>(_mapper.ConfigurationProvider)
               .ToListAsync();

            return friends;
        }

        public async Task<SelectItemVm> GetAllAsSelect(FriendDto friendDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Friends
                    .Where(x => !x.Deleted)
                    .Select(x => new SelectItemDto { Label = x.Nickname, Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }
    }
}
