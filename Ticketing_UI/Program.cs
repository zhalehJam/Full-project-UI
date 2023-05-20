using Blazored.Modal; 
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ticketing_UI;
using Ticketing_UI.Shared.Classes;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("API"));
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["HttpClient:BaseAddress"]!);
    client.Timeout = new TimeSpan(0, 0, 0, 30);
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();


builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = builder.Configuration["IdentityServer:Authority"];
    options.ProviderOptions.ClientId = "Ticketing";
    options.ProviderOptions.RedirectUri = builder.Configuration["IdentityServer:RedirectUri"];
    options.ProviderOptions.PostLogoutRedirectUri = builder.Configuration["IdentityServer:PostLogoutRedirectUri"];
    options.UserOptions.RoleClaim = "role";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("Ticketing.API");
    options.ProviderOptions.DefaultScopes.Add("offline_access");
    options.ProviderOptions.DefaultScopes.Add("Ticketing_Identity_Resource");
    options.ProviderOptions.DefaultScopes.Add("roles");

});
//builder.Services.AddApiAuthorization();

builder.Services.AddBlazoredModal();

Registrar.RegisterRepositories(builder.Services);

//Registrar registrar = new Registrar(builder.Services);
//registrar.Register();

var app = builder.Build();


await builder.Build().RunAsync();
