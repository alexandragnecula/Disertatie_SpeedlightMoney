using System;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class ApplicationRole : IdentityRole<long>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name) : base(name)
        {
        }
    }
}
