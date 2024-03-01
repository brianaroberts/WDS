using WDS.Authentication;
using WDSClient;
using WDSClient.Models.Users;

namespace WDSWebClient
{
    /// <summary>
    /// To be used just on the web. 
    /// </summary>
    public class WDSWebClientReader : WDSClientReader
    {
        #region Constructors
        
        public WDSWebClientReader(Microsoft.AspNetCore.Http.HttpRequest request) 
        {
            //TODO: Web App needs to redirect to unauthroized. Only individuals with EDIPI's can use the web service. 
            if (!request.Headers.TryGetValue(HttpHeaderNameConstants.CallerIdentity, out var userIds))
                throw new ArgumentException("User ID is required to use the WDS Client");

            Application = SettingsReader.GetAppSetting("AppKeys:WDSClientSettings", "Application");
            APIKey = SettingsReader.GetAppSetting("AppKeys:WDSClientSettings", "API-Key");
            UserId = new UserIdentifier(userIds.FirstOrDefault(""));  // This should be in the format ##########.A
        }
        
        #endregion
    }

    public class WebClient : Client
    {
        public WebClient(IWDSClientReader reader): base(reader)
        {

        }

        #region JavaScript
        public string GenerateJavaScriptHook(string onLoadFunctionScript, bool needScriptTags = false)
        {
            var returnValue = @$"var request = new XMLHttpRequest();
                request.open('GET', '{WDSServer}')
                request.setRequestHeader('{HttpHeaderNameConstants.SessionKey}','{User.SessionKey}')
                request.setRequestHeader('{HttpHeaderNameConstants.CallerIdentity}','{User.EDIPI}')
                request.send();
                request.onload = 
                    {onLoadFunctionScript} ";

            return needScriptTags ? JavaScriptTags(returnValue) : returnValue;
        }

        private string JavaScriptTags(string input)
        {
            return $"<script>{input}</script>";
        }
        #endregion
    }
}