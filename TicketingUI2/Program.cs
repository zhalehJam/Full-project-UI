using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using TicketingUI2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //var builder = WebAssemblyHostBuilder.CreateDefault(args);
        //builder.RootComponents.Add<App>("#app");
        //builder.RootComponents.Add<HeadOutlet>("head::after");

        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        //builder.Services.AddOidcAuthentication(options =>
        //{
        //    // Configure your authentication provider options here.
        //    // For more information, see https://aka.ms/blazor-standalone-auth
        //    builder.Configuration.Bind("Local", options.ProviderOptions);
        //});

        //await builder.Build().RunAsync();
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory >()
            .CreateClient("API"));
        //builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["HttpClient:BaseAddress"]);
            client.Timeout = new TimeSpan(0, 0, 0, 30);
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });//.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();




        builder.Services.AddOidcAuthentication(options =>
        {
            options.ProviderOptions.Authority = builder.Configuration["IdentityServer:Authority"];
            options.ProviderOptions.ClientId = "TMS";
            options.ProviderOptions.RedirectUri = builder.Configuration["IdentityServer:RedirectUri"];
            options.ProviderOptions.PostLogoutRedirectUri = builder.Configuration["IdentityServer:PostLogoutRedirectUri"];
            options.UserOptions.RoleClaim = "role";
            options.ProviderOptions.ResponseType = "code";
            options.ProviderOptions.DefaultScopes.Add("TMS.API");
            options.ProviderOptions.DefaultScopes.Add("offline_access");
            options.ProviderOptions.DefaultScopes.Add("TMS_Identity_Resource");
            options.ProviderOptions.DefaultScopes.Add("roles");
        });
        builder.Services.AddApiAuthorization();
        //builder.Services.AddBlazoredModal();
        //builder.Services.AddFontAwesomeIcons();


        var host = builder.Build();
        //Registrar registrar = new Registrar(builder.Services);
        //registrar.Register();
        await builder.Build().RunAsync();
    }
}