using Newtonsoft.Json;
using WDS.Models;

namespace DataService.Models.User
{
    public class UserDTO : WDSClient.Models.Users.IUser
    {
        [JsonProperty("sessionKey")]
        public string SessionKey { get; set; }
        private string _edipi;
        [JsonProperty("edipi")]
        public string EDIPI
        {
            get
            {
                return _edipi;
            }
            set
            {
                _edipi = value;
            }
        }
        private char _ptc;
        [JsonProperty("ptc")]
        public char PTC
        {
            get
            {
                return _ptc;
            }
            set
            {
                _ptc = value;
            }
        }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("mid_name")]
        public string MiddleInitial { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("svc_branch")]
        public string ServiceBranch { get; set; }
        [JsonProperty("rank")]
        public string Rank { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("division")]
        public string Division { get; set; }
        [JsonProperty("office")]
        public string Office { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("fullName")]
        public string Fullname
        {
            get
            {
                return $"{FirstName} {MiddleInitial} {LastName}";
            }
        }
       
        public UserDTO()
        {

        }
    }
}
