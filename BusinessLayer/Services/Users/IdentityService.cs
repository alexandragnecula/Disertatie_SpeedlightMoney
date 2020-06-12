using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Common.Constants;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Services.Wallets;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using DataLayer.SharedInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services.Users
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly IWalletService _walletService;
        private readonly ICurrentUserService _currentUserService;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            DatabaseContext context, JwtSettings jwtSettings,
            IMapper mapper, IWalletService walletService,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _jwtSettings = jwtSettings;
            _mapper = mapper;
            _walletService = walletService;
            _currentUserService = currentUserService;
        }

        public async Task<string> GetUserNameAsync(long userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<Result> CreateRoleAsync(string name)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(name));
            return (result.ToApplicationResult());
        }

        public async Task<Result> AddToRoleAsync(long userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var result = await _userManager.AddToRoleAsync(user, role);

            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteUserAsync(long userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.IsActive = false;
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<long?> GetUserIdAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user?.Id;
        }

        public async Task<(Result, long UserId)> CreateUserSeedAsync(UserDto userToAdd)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = userToAdd.Email,
                    Email = userToAdd.Email,

                    FirstName = userToAdd.FirstName,
                    LastName = userToAdd.LastName,
                    Birthdate = userToAdd.Birthdate,
                    CNP = userToAdd.CNP,
                    Country = userToAdd.Country,
                    County = userToAdd.County,
                    City = userToAdd.City,
                    StreetName = userToAdd.StreetName,
                    StreetNumber = userToAdd.StreetNumber,
                    CurrentStatus = userToAdd.CurrentStatus,
                    CardNumber = userToAdd.CurrentStatus,
                    Cvv = userToAdd.Cvv,
                    ExpireDate = userToAdd.ExpireDate,
                    Salary = userToAdd.Salary,
                    PhoneNumber = userToAdd.PhoneNumber,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, userToAdd.Password);

                var role = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == userToAdd.RoleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    throw new Exception("The role doesn't exist!");
                }

                await _context.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync();

                return (result.ToApplicationResult(), user.Id);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new Exception(e.ToString());
            }
        }

        public async Task<AuthenticationResult> RegisterAsync(UserDto userToAdd)
        {
            var existingUser = await _userManager.FindByEmailAsync(userToAdd.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = userToAdd.Email,
                    Email = userToAdd.Email,

                    FirstName = userToAdd.FirstName,
                    LastName = userToAdd.LastName,
                    Birthdate = userToAdd.Birthdate,
                    CNP = userToAdd.CNP,
                    Country = userToAdd.Country,
                    County = userToAdd.County,
                    City = userToAdd.City,
                    StreetName = userToAdd.StreetName,
                    StreetNumber = userToAdd.StreetNumber,
                    CurrentStatus = userToAdd.CurrentStatus,
                    CardNumber = userToAdd.CardNumber,
                    Cvv = userToAdd.Cvv,
                    ExpireDate = userToAdd.ExpireDate,
                    Salary = userToAdd.Salary,
                    PhoneNumber = userToAdd.PhoneNumber,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, userToAdd.Password);

                if (!result.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Errors = result.Errors.Select(x => x.Description)
                    };
                }

                var role = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == userToAdd.RoleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    return new AuthenticationResult
                    {
                        Errors = new List<string> { "The role doesn't exist!" }
                    };
                }

                await _context.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync();
                return await GenerateAuthenticationResultForUserAsync(user);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new Exception(e.ToString());
            }
        }

        public async Task<AuthenticationResult> LoginAsync(LoginUserDto loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, loginUser.Password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User/password combination is wrong" }
                };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("id", user.Id.ToString())
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            await _context.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<Result> UpdateUser(UserDto userToUpdate)
        {
            var entity = await _context.Users.FindAsync(userToUpdate.Id);

            if (entity == null)
            {
                return Result.Failure(new List<string> { "No valid user found" });
            }

            entity.Email = userToUpdate.Email;
            entity.FirstName = userToUpdate.FirstName;
            entity.LastName = userToUpdate.LastName;
            entity.Birthdate = userToUpdate.Birthdate;
            entity.CNP = userToUpdate.CNP;
            entity.Country = userToUpdate.Country;
            entity.County = userToUpdate.County;
            entity.City = userToUpdate.City;
            entity.StreetName = userToUpdate.StreetName;
            entity.StreetNumber = userToUpdate.StreetNumber;
            entity.CurrentStatus = userToUpdate.CurrentStatus;
            entity.CardNumber = userToUpdate.CardNumber;
            entity.Cvv = userToUpdate.Cvv;
            entity.ExpireDate = userToUpdate.ExpireDate;
            entity.Salary = userToUpdate.Salary;
            entity.PhoneNumber = userToUpdate.PhoneNumber;
            await _context.SaveChangesAsync();
            await AddUserToRole(userToUpdate.Id, userToUpdate.RoleId);

            return Result.Success("User update was successful");
        }

        public async Task AddUserToRole(long userId, long roleId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive.Value);
            var roles = await _roleManager.Roles.ToListAsync();
            var existingRoleNames = await _userManager.GetRolesAsync(user);
            

            await _userManager.RemoveFromRoleAsync(user, existingRoleNames[0]);

            var roleToAdd = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            await _userManager.AddToRoleAsync(user, roleToAdd.Name);
        }

        public async Task<UserDto> GetUserById(long id)
        {
            var userRoleId = await _context.UserRoles
                               .FirstOrDefaultAsync(x => x.UserId == id);

            var entity = await _context.Users
                .FindAsync(id);

            //return entity == null ? null : _mapper.Map<UserDto>(entity);
            var entityVm = entity == null ? null : _mapper.Map<UserDto>(entity);

            if (entityVm != null)
                entityVm.RoleId = userRoleId.RoleId;

            return entityVm;
        }

        public async Task<IList<UserDto>> GetAllUsers()
        {
            List<UserDto> users = await _context.Users
               .Where(x => x.Id != _currentUserService.UserId.Value)
               .OrderByDescending(x => x.Email)
               .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
               .ToListAsync();

            return users;
        }

        public async Task<SelectItemVm> GetAllAsSelect(UserDto userDto)
        {
            var vm = new SelectItemVm
            {
                SelectItems = await _context.Users
                    .Where(x => x.IsActive == true && x.Id != _currentUserService.UserId.Value)
                    .Select(x => new SelectItemDto { Label = x.GetFullName(), Value = x.Id.ToString() })
                    .ToListAsync()
            };

            return vm;
        }

        public async Task<AuthenticationResult> RegisterAsyncWithWallets(RegisterUserDto userToAdd)
        {
            var existingUser = await _userManager.FindByEmailAsync(userToAdd.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = userToAdd.Email,
                    Email = userToAdd.Email,

                    FirstName = userToAdd.FirstName,
                    LastName = userToAdd.LastName,
                    Birthdate = userToAdd.Birthdate,
                    CNP = userToAdd.CNP,
                    Country = userToAdd.Country,
                    County = userToAdd.County,
                    City = userToAdd.City,
                    StreetName = userToAdd.StreetName,
                    StreetNumber = userToAdd.StreetNumber,
                    CurrentStatus = userToAdd.CurrentStatus.ToString(),
                    CardNumber = userToAdd.CardNumber,
                    Cvv = userToAdd.Cvv,
                    ExpireDate = userToAdd.ExpireDate,
                    Salary = userToAdd.Salary,
                    PhoneNumber = userToAdd.PhoneNumber,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, userToAdd.Password);

                if (!result.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Errors = result.Errors.Select(x => x.Description)
                    };
                }

                var role = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == userToAdd.RoleId);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    return new AuthenticationResult
                    {
                        Errors = new List<string> { "The role doesn't exist!" }
                    };
                }

                var wallet = new Wallet
                {
                    TotalAmount = userToAdd.TotalAmount,
                    UserId = user.Id,
                    CurrencyId = userToAdd.CurrencyId
                };

                if (wallet.CurrencyId == 1) {
                    var walletEUR = new Wallet
                    {
                        TotalAmount = 0.0,
                        UserId = user.Id,
                        CurrencyId = 2
                    };
                    await _context.Wallets.AddAsync(walletEUR);
                }
                else if (wallet.CurrencyId == 2)
                {
                    var walletRON = new Wallet
                    {
                        TotalAmount = 0.0,
                        UserId = user.Id,
                        CurrencyId = 1
                    };
                    await _context.Wallets.AddAsync(walletRON);
                }

                await _context.Wallets.AddAsync(wallet);

                await _context.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync();
                return await GenerateAuthenticationResultForUserAsync(user);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw new Exception(e.ToString());
            }
        }

        public async Task<SelectItemVm> GetCurrentStatusesAsSelect()
        {
            var items = new List<SelectItemDto>();

            CurrentStatusesList st = new CurrentStatusesList();
            foreach(var i in st.CurrentStatuses)
            {
                items.Add(new SelectItemDto { Label = i, Value = i });
            }

            return await Task.FromResult(new SelectItemVm { SelectItems = items });
        }
    }
}
