using System.Collections.Generic;
using System;

namespace DataService.Models.User
{
    public class ApplicationUserDTO : UserDTO
    {
        public List<ApplicationUserRoleDTO> Roles { get; set; }
        public List<ClaimsDTO> Claims { get; set; }
        public DateTime LastAccessed { get; set; }

        public ApplicationUserDTO() { }
        public ApplicationUserDTO(UserDTO user)
        {
            if (user == null) return;
            this.SessionKey = user.SessionKey;
            this.EDIPI = user.EDIPI;
            this.PTC = user.PTC;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.MiddleInitial = user.MiddleInitial;
            this.DisplayName = user.DisplayName;
            this.ServiceBranch = user.ServiceBranch;
            this.Rank = user.Rank;
            this.Title = user.Title;
            this.Organization = user.Organization;
            this.Office = user.Office;
            this.Phone = user.Phone;
            this.Division = user.Division;
            this.Email = user.Email;
        }
    }
}
