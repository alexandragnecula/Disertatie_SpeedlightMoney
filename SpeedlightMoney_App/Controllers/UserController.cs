using System;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.Authentication;
using BusinessLayer.Services.Users;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        IIdentityService _identityService;
        public UserController(IIdentityService identityService)
        {
             _identityService  = identityService;
        }

        [HttpPost("adduser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userToAdd)
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
    }
}
