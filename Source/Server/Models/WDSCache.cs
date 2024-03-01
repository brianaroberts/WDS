using System;
using System.Collections.Generic;
using System.Linq;
using DataService.Models.User; 

namespace DataService.Models
{
    public class SessionInfo
    {
        public string ApplicationKey { get; set; }
        public DateTime Expires { get; set; }

        public SessionInfo()
        {
            
        }
    }

    public class UserInfo
    {
        public UserDTO Info { get; set; }
        public DateTime Expires { get; set; }
    }

    public static class WDSCache
    {
        public static string WD_CACHE_ID = "WDSCache";
        public static string WD_CACHE_FILE_EXTENSION = "wds";
        public static string CACHED_DATA_ENCRYPT_KEY = "klda;['elskdiles";
        private static int SESSION_TIMEOUT_HOURS = 4;
        private static int USER_CACHE_TIMEOUT_HOURS = 4;

        private static Dictionary<string, SessionInfo> Sessions = new Dictionary<string, SessionInfo>();
        private static Dictionary<string, UserInfo> Users = new Dictionary<string, UserInfo>();

        public static void CleanCache()
        {
            var sessionsToRemove = Sessions.Where(o => o.Value.Expires < DateTime.Now).Select(o=>o.Key);
            foreach (var key in sessionsToRemove)
                Sessions.Remove(key);

            var usersToRemove = Users.Where(o => o.Value.Expires < DateTime.Now).Select(o => o.Key);
            foreach (var key in usersToRemove)
                Users.Remove(key);
        }

        public static bool TryGetSession(string sessionId, out SessionInfo session)
        {
            var returnValue = false;
            if (Sessions.ContainsKey(sessionId))
            {
                session = Sessions[sessionId];
                returnValue = true;
            }
            else
                session = null; 

            return returnValue; 
        }

        public static void AddSession(string sessionKey, string applicationKey)
        {
            if (Sessions.Count > 1000)
            {
                var removeKey = Sessions.First().Key;
                Sessions.Remove(removeKey);
            }

            if (!Sessions.ContainsKey(sessionKey))
                Sessions.Add(sessionKey, new SessionInfo());

            Sessions[sessionKey].ApplicationKey = applicationKey;
            Sessions[sessionKey].Expires = DateTime.Now.AddHours(SESSION_TIMEOUT_HOURS); 
        }

        public static bool TryGetUsers(string edipi, out UserDTO user)
        {
            var returnValue = false;
            if (Users.ContainsKey(edipi))
            {
                user = Users[edipi].Info;
                returnValue = true;
            }
            else
                user = null;

            return returnValue;
        }

        public static void AddUser(string key, UserDTO user)
        {
            if (Users.Count > 1000)
            {
                var removeKey = Users.First().Key;
                Users.Remove(removeKey);
            }

            if (!Users.ContainsKey(key))
                Users.Add(key, new UserInfo());

            Users[key].Info = user; Users[key].Expires = DateTime.Now.AddHours(USER_CACHE_TIMEOUT_HOURS);
        }
    }
}
