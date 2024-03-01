using WDS.Models;

namespace WDSClient.Models.Users
{
    public class ApplicationUser : User, IApplicationUser
    {
        public List<IApplicationUserRole> Roles { get; set; }
        public List<IClaims> Claims { get; set; }
        public DateTime LastAccessed { get; set; }
        
        public ApplicationUser() { }
        public ApplicationUser(string message)
        {
            ErrorMessage = message;
        }
        public ApplicationUser(User user)
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

        public static ApplicationUser GetApplicationUser(UserIdentifier dodId, string applicationName, Client client)
        {
            var request = new WDSRequestContract("Authorization.GetApplicationUser");
            var requestURL = $"Authorization/GetApplicationUser?userId={dodId.EDIPI}.{dodId.PTC}&applicationName={applicationName}";
            ApplicationUser returnValue = null;

            try
            {
                returnValue = client.MakeCall<ApplicationUser>(requestURL, request);
            }
            catch (Exception ex)
            {
                returnValue = new ApplicationUser(ex.Message);
            }
            return returnValue;
        }
    }
}
