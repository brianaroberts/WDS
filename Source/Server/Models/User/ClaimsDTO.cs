using WDSClient.Models.Users;

namespace DataService.Models.User
{
    public class ClaimsDTO : IClaims
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
