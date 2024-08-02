using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace StaticWebAppAuthentication.Client
{
    public class StaticWebAppsAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;

        public StaticWebAppsAuthenticationStateProvider(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var clientPrincipal = await GetClientPrinciple();
                if (clientPrincipal is null) 
                {
					return new AuthenticationState(new ClaimsPrincipal());
				}
				var claimsPrincipal = GetClaimsFromClientClaimsPrincipal(clientPrincipal);
                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        private async Task<ClientPrincipal?> GetClientPrinciple()
        {
            var data = await _http.GetFromJsonAsync<AuthenticationData>("/.auth/me");
            var clientPrincipal = data?.ClientPrincipal ??null;
            return clientPrincipal;
        }

        private static ClaimsPrincipal GetClaimsFromClientClaimsPrincipal(ClientPrincipal principal)
        {
            var filteredRoles =
				principal.UserRoles?.Except(["anonymous"], StringComparer.CurrentCultureIgnoreCase) ?? [];

            if (!filteredRoles.Any())
            {
                return new ClaimsPrincipal();
            }

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
            identity.AddClaims(filteredRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            return new ClaimsPrincipal(identity);
        }
    }
}
