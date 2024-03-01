using System;
using WDSClient.Models.Users;

namespace DataService.Models.User
{
    public class ApplicationUserRoleDTO : IApplicationUserRole
    {
        public string Role { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive { get; set; }
    }
}
