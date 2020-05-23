using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.Roles
{
    public interface IRoleService
    {
        Task<Result> AddRoleAsync(AddRoleDto roleToAdd);
    }
}
