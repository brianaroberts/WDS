using Microsoft.AspNetCore.Http;
using System.Linq;
using WDS.Authentication;

namespace DataService.Core
{
    public static class AuthenticationCommon
    {
        

        public static string GetAPIKey(HttpRequest request)
        {
            var exists = request.Headers.TryGetValue(HttpHeaderNameConstants.ApiKey, out var key);

            var returnValue = (key.Count > 0) ? key.FirstOrDefault("") : "";


            return returnValue;
        }

        public static string GetCallerId(HttpRequest request)
        {
            var exists = request.Headers.TryGetValue(HttpHeaderNameConstants.CallerIdentity, out var key);

            var returnValue = (key.Count > 0) ? key.FirstOrDefault("") : "";

            return returnValue;
        }
    }
}
