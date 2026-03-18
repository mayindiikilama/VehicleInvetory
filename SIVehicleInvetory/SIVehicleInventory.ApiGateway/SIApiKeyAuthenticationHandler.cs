using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SIVehicleInventory.ApiGateway
{
    public class SIApiKeyAuthenticationHandler : AuthenticationHandler<SIApiKeyAuthenticationOptions>
    {
        public SIApiKeyAuthenticationHandler(
            IOptionsMonitor<SIApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // STEP 1: CHECK IF HEADER EXIST
            if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKeyValues))
            {
                // If no API key then reject request
                return Task.FromResult(AuthenticateResult.Fail("Missing API Key"));
            }

            
            // STEP 2: VALIDATE API KEY

            var apiKey = apiKeyValues.First();

            // my api key
            if (apiKey != "Samson123")
            {
                // Wrong key entered will be rejected
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
            }


            // STEP 3: CREATE AUTHENTICATED USER
            
            // Create identity of the person who is calling the API
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "ApiGatewayUser")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);

            // Wrap identity into a principal (standard ASP.NET user object)
            var principal = new ClaimsPrincipal(identity);

            // Create authentication ticket (final result)
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return success means request continues
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    // Options class 
    public class SIApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
}