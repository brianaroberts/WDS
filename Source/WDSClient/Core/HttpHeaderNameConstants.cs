namespace WDS.Authentication
{
    // Needed to make the web requests to the WDS instance server
    public class HttpHeaderNameConstants
    {
        public const string ApiKey = "x-api-key";
        public const string CallerIdentity = "EDIPI";
        public const string SessionKey = "X-Session-Key"; 
    }
}
