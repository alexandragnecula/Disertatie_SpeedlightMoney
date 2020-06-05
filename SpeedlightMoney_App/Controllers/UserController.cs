using System;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.Authentication;
using BusinessLayer.Services.Users;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BusinessLayer.Common.Models.SelectItem;
using Newtonsoft.Json.Linq;
using BusinessLayer.Common.Constants;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    //[Produces("application/json")]
    public class UserController : ControllerBase
    {
        IIdentityService _identityService;
        public UserController(IIdentityService identityService)
        {
             _identityService  = identityService;
        }

        [HttpPost("adduser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto userToAdd)
        {
            var authResponse = await _identityService.RegisterAsync(userToAdd);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        [HttpPost("adduserandwallets")]
        public async Task<IActionResult> AddUserAndWallet([FromBody] RegisterUserDto userToAdd)
        {
            var authResponse = await _identityService.RegisterAsyncWithWallets(userToAdd);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            var authResponse = await _identityService.LoginAsync(loginUser);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
            });
        }

        // GET
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Result>> UpdateUser([FromBody] UserDto userDto)
        {
            var response = await _identityService.UpdateUser(userDto);

            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById(long id)
        {
            var vm = await _identityService.GetUserById(id);

            if (vm == null)
            {
                return BadRequest(Result.Failure(new List<string> { "No valid user was found" }));
            }

            return Ok(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Ultimate, Premium")]
        public async Task<ActionResult<UserDto>> GetUsers()
        {
            var vm = await _identityService.GetAllUsers();

            return Ok(vm);
        }

        [HttpGet("usersdropdown")]
        [Authorize]
        public async Task<ActionResult<SelectItemVm>> GetUsersDropdown()
        {
            var vm = await _identityService.GetAllAsSelect(new UserDto());

            return Ok(vm);
        }

        [HttpGet("currentstatusesdropdown")]
        public async Task<ActionResult<SelectItemVm>> GetCurrentStatusesDropdown()
        {
            var vm = await _identityService.GetCurrentStatusesAsSelect();

            return Ok(vm);
        }
    }
}
