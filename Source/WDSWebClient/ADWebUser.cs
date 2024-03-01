using Microsoft.AspNetCore.Http;
using WDSClient.Models.Users;

namespace WDSWebClient
{
    public static class ADWebUser
    {
        public static User GetUserUsingAD(IHttpContextAccessor httpContextAccessor, WDSClient.Client client)
        {
            var userName = httpContextAccessor.HttpContext.User.Identity.Name;
            var returnUser = ADUser.GetUserUsingAD(userName, client); 
            
            return returnUser;
        }
    }
}
