using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Utilities;
using BusinessLayer.Views;
using DataLayer.DataContext;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DatabaseContext _context;

        public RoleService(RoleManager<ApplicationRole> roleManager, DatabaseContext context)
        {       
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<Result> AddRoleAsync(RoleDto roleToAdd)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleToAdd.Name);

            if (existingRole != null)
            {
                throw new ArgumentException("A role with this name already exists");
              
               
            }

            var role = new ApplicationRole
                {
                    Name = roleToAdd.Name

                };

                var result = await _roleManager.CreateAsync(role);

                await _context.SaveChangesAsync();

                return (result.ToApplicationResult());
         }
    }
}
