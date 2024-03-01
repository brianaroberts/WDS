using Microsoft.AspNetCore.Authentication;
using System;

namespace WDS.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<WDSAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<WDSAuthenticationOptions, WDSAuthenticationHandler>(WDSAuthenticationOptions.DefaultScheme, options);
        }
    }
}
