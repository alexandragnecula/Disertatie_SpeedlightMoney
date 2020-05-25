using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Friends;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FriendController : ControllerBase
    {
        IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("addfriend")]
        public async Task<IActionResult> AddCurrency([FromBody] FriendDto friendToAdd)
        {
            if (friendToAdd.Nickname == null)
            {
                return BadRequest("The nickname is mandatory!");
            }
            var result = await _friendService.AddFriend(friendToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UpdateCurrecy([FromBody] FriendDto friendToUpdate)
        {
            if (friendToUpdate.Nickname == null)
            {
                return BadRequest("The nickname is mandatory!");
            }

            var result = await _friendService.UpdateFriend(friendToUpdate);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{friendId}")]
        public async Task<ActionResult<Result>> DeleteFriend(long friendId)
        {
            var result = await _friendService.DeleteFriend(new FriendDto { Id = friendId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("restore")]
        public async Task<ActionResult<Result>> RestoreFriend(long friendId)
        {
            var result = await _friendService.RestoreFriend(new FriendDto { Id = friendId });

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FriendDto>> GetFriendById(long id)
        {
            var vm = await _friendService.GetFriendById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid friend was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        public async Task<ActionResult<FriendDto>> GetAllFriends()
        {
            var vm = await _friendService.GetAllFriends();

            return Ok(vm);
        }

        [HttpGet("friendsdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetFriendsDropdown()
        {
            var vm = await _friendService.GetAllAsSelect(new FriendDto());

            return Ok(vm);
        }
    }
}
