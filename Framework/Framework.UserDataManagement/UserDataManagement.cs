using Framework.Core.UserDataManagement;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Framework.UserDataManagement
{
    public class UserDataManagement : IUserDataManagement
    {
        private ClaimsPrincipal user;
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private AuthenticationState tt;

        public UserDataManagement(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            
             SetUser();
        }

        private async Task SetUser()
        {
            tt = authenticationStateProvider.GetAuthenticationStateAsync().Result; 
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            user = authState.User;
            claims = user.Claims;
             var isinvalidtoken = _isEmptyOrInvalid(tt.User.FindFirst("access_token")?.Value);
            
        }

        public bool IsInRole(string role)
        {
            SetUser();
            return claims.Any(c => c.Type == "role" && c.Value.Contains(role));
        }

        public bool _isEmptyOrInvalid(string token)
        {

            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            var jwtToken = new JwtSecurityToken(token);
            return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
        }
    }
}