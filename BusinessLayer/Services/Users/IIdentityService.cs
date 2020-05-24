using System;
using System.Threading.Tasks;
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
        //Task<(Result, long UserId)> CreateUserSeedAsync(AddUserCommand userToAdd);
    }
}
