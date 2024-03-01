using Microsoft.AspNetCore.Authentication;

namespace WDS.Authentication
{
    public class WDSAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
