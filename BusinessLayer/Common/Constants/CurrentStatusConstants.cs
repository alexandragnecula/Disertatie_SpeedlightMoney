using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BusinessLayer.Common.Constants
{
    public class CurrentStatusConstants
    {
        public const string Student = "Student";
        public const string Employee = "Employee";
        public const string StudentEmployee = "Student & Employee";
        public const string Retired = "Retired";
        public const string Other = "Other";

        public CurrentStatusConstants()
        {

        }
    }

    public class CurrentStatusesList
    {
        public CurrentStatusesList()
        {
            CurrentStatuses = new List<string> { CurrentStatusConstants.Student,
                                                         CurrentStatusConstants.Employee,
                                                         CurrentStatusConstants.StudentEmployee,
                                                         CurrentStatusConstants.Other
                                                        };
        }
        public List<string> CurrentStatuses { get; }
    }
}
