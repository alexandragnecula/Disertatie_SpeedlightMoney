﻿using System;
using System.Threading.Tasks;
using BusinessLayer.Services.Roles;
using BusinessLayer.Views;
using Microsoft.AspNetCore.Mvc;

namespace SpeedlightMoney_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddUser([FromBody] AddRoleDto roleToAdd)
        {
            var result = await _roleService.AddRoleAsync(roleToAdd);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
