using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DataService.Core;
using WDS.Core.Extensions;
using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace WDS.Authentication
{
    public class WDSAuthenticationHandler : AuthenticationHandler<WDSAuthenticationOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private readonly IGetApiKeyQuery _getAPIKeyQuery;
        
        public WDSAuthenticationHandler(IOptionsMonitor<WDSAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IGetApiKeyQuery getApiKeyQuery) : base(options, logger, encoder, clock)
        {
            _getAPIKeyQuery = getApiKeyQuery ?? throw new ArgumentNullException(nameof(getApiKeyQuery));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var host = Request.HttpContext.Request.Host.Host;
            
            var providedApiKey = AuthenticationCommon.GetAPIKey(Request);
            var providedUserId = AuthenticationCommon.GetCallerId(Request);

            if (providedApiKey.IsNullOrEmpty() && providedUserId.IsNullOrEmpty())
            {
                return AuthenticateResult.NoResult();
            }

            var existingApiKey = await _getAPIKeyQuery.Execute(providedApiKey);
            var ipAddressMatch = DataServiceSettings.ApiKeys.DoesKeyExist(providedApiKey) ? 
                host.IsMatch(DataServiceSettings.ApiKeys.GetIPMask(providedApiKey)) 
                : false;

           // API Key Authentication
            var authenticationType = Options.AuthenticationType;  // value = "API Key" set elsewhere. 
           
            // API Key Autentication

            if (existingApiKey != null && ipAddressMatch)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name.ToString(), existingApiKey.Owner.ToString())
                };

                claims.AddRange(existingApiKey.Roles.Select(Roles => new Claim(ClaimTypes.Role, Roles)));

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.NoResult();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new UnauthorizedProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            var problemDetails = new ForbiddenProblemDetails();

            await Response.WriteAsync(JsonSerializer.Serialize(problemDetails, DefaultJsonSerializerOptions.Options));
        }
    }
}
