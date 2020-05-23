using System;
using System.Collections.Generic;

namespace BusinessLayer.Common.Constants
{
    public class RoleConstants
    {
        public const string Admin = "Admin";
        public const string Explorer = "Explorer";
        public const string Premium = "Premium";
        public const string Ultimate = "Ultimate";
    }

    public class RoleList
    {
        public RoleList()
        {
            Roles = new List<string> { RoleConstants.Admin, RoleConstants.Explorer, RoleConstants.Premium, RoleConstants.Ultimate };
        }
        public List<string> Roles { get; }
    }
}
