using Framework.Core.UserDataManagement;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Framework.UserDataManagement
{
    public class UserDataManagement : IUserDataManagement
    {
        private ClaimsPrincipal user;
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
        private readonly AuthenticationStateProvider authenticationStateProvider; 
        public UserDataManagement(AuthenticationStateProvider authenticationStateProvider )
        {
            this.authenticationStateProvider = authenticationStateProvider; 
            SetUser();
        }

        private async Task SetUser()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            user = authState.User;
            claims = user.Claims;
        }

        public bool IsInRole(string role)
        {
            SetUser();
            return claims.Any(c => c.Type == "role" && c.Value.Contains(role));
        }
    }
}