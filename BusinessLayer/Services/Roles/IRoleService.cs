using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Common.Models.SelectItem;
using BusinessLayer.Utilities;
using BusinessLayer.Views;

namespace BusinessLayer.Services.Roles
{
    public interface IRoleService
    {
        Task<Result> AddRoleAsync(RoleDto roleToAdd);
        Task<SelectItemVm> GetAllAsSelect(RoleDto roleDto);
    }
}
