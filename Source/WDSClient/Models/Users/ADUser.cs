using System.DirectoryServices;
using WDS.Core.Extensions;

namespace WDSClient.Models.Users
{
    public static class ADUser
    {
        public static User GetUserUsingAD(string userName, Client client)
        {
            User returnUser = null;

            string edipi = GetEDIPIUsingAD(userName, client); 

            if (!edipi.IsNullOrEmpty())
            {
                returnUser = User.GetUser(new UserIdentifier(edipi), client);
            }
               
            return returnUser;
        }
        public static string GetEDIPIUsingAD(string userName, Client client)
        {
            var userLookup = userName[(userName.IndexOf("\\") + 1)..];
            string edipi = ""; 

            //TODO: Go to the DataService to get this value. 
            string appPartitionConnectionString = SettingsReader.AppSetting("AppKeys:ActiveDirectory", "LDAP.PartitionConnectionString");

            using (DirectoryEntry dirEntry = new(appPartitionConnectionString))
            {
                DirectorySearcher ds = new(dirEntry)
                {
                    Filter = "(sAMAccountName=" + userName + ")"
                };

                SearchResult userFromAD = ds.FindOne();
                if (userFromAD != null)
                {
                    DirectoryEntry currEntry = userFromAD.GetDirectoryEntry();
                    edipi = currEntry.Properties["employeeid"].Value.ToString();
                }
            }
            return edipi; 
        }
    }
}
