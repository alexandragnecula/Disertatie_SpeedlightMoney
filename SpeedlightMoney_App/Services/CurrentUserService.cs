using System;
using System.Security.Claims;
using DataLayer.SharedInterfaces;
using Microsoft.AspNetCore.Http;

namespace SpeedlightMoney_App.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
            if (!string.IsNullOrEmpty(id))
            {
                UserId = long.Parse(id);
            }
        }

        public long? UserId { get; }
    }
}
