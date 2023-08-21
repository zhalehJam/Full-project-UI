using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ticketing_Client.Pages
{
    public class LoginModelModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string redirectUri)
        {
            // just to remove compiler warning
            await Task.CompletedTask;
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = "https://login.shonizcloud.ir";
            }
            // If user is already logged in, we can redirect directly...
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //Response.Redirect(redirectUri);
            }
             

            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri =""
                },
                OpenIdConnectDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme); 
        }
    }
}
