using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components; 
//using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace Ticketing_Client.Shared.Classes
{
    public class CustomAuthorizationMessageHandler  
    {
        public CustomAuthorizationMessageHandler( 
            NavigationManager navigationManager,
            IConfiguration configuration)
            
        {
            //ConfigureHandler(
            //    authorizedUrls: new[] { configuration["IdentityServer:AuthorizedUrls"] },
            //    scopes: new[] { "Ticketing.API", "Ticketing_Identity_Resource", "roles", "profile", "openid" });

        }
    }
}
