using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Common.Constants;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.Entities;

namespace BusinessLayer.Services.Users
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(long userId);

        Task<Result> DeleteUserAsync(long userId);

        Task<Result> CreateRoleAsync(string name);
        Task<Result> AddToRoleAsync(long userId, string role);
        Task<bool> RoleExistsAsync(string role);
        Task<long?> GetUserIdAsync(string email);
        Task<AuthenticationResult> LoginAsync(LoginUserDto loginUser);
        Task<AuthenticationResult> RegisterAsync(UserDto userToAdd);
        Task<(Result, long UserId)> CreateUserSeedAsync(UserDto userToAdd);
        Task<Result> UpdateUser(UserDto userToUpdate);
        Task<UserDto> GetUserById(long id);
        Task<IList<UserDto>> GetAllUsers();
        Task<SelectItemVm> GetAllAsSelect(UserDto userDto);
        Task<AuthenticationResult> RegisterAsyncWithWallets(RegisterUserDto userToAdd);
        Task<SelectItemVm> GetCurrentStatusesAsSelect();
    }
}
