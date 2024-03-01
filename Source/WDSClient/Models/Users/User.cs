using System.Net.Http;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using WDS.Core.Extensions;
using WDS.Models;

namespace WDSClient.Models.Users
{
    public class User : IUser
    {
        
        public string SessionKey { get; set; }
        private string _edipi;
        
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
                
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string DisplayName { get; set; }
        public string ServiceBranch { get; set; }
        public string Rank { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string Division { get; set; }
        public string Office { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fullname
        {
            get
            {
                return $"{FirstName} {MiddleInitial} {LastName}";
            }
        }
        protected string ErrorMessage { get; set; }

        public static User GetUser(UserIdentifier userId, Client client)
        {
            var request = new WDSRequestContract("Authorization.GetUser");
            return client.MakeCall($"Authorization/GetUser?edipi={userId.EDIPI}&ptc={userId.PTC}", request).GetMappedData<User>();
        }

        public static User GetUser()
        {
            return new User(); 
        }

        public User()
        {

        }

        public override string ToString()
        {
            var returnValue = ErrorMessage.IsNullOrEmpty() ? 
                    $"{DisplayName} ({FirstName} {MiddleInitial} {LastName}) {Title} ({Rank}) {Organization} {Division} {Office} {Phone}" : ErrorMessage;
            return returnValue;
        }
    }
}
