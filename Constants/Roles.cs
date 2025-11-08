using System.Collections.Generic;

namespace TaskManagementSystem.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        public static List<string> GetAllRoles()
        {
            return new List<string> { Admin, Manager, User };
        }
    }
}