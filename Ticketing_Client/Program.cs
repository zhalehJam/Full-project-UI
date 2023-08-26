using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor;
using Ticketing_Client;
using Ticketing_Client.Data;
using Ticketing_Client.Shared.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Ticketing.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

Registrar.RegisterRepositories(builder.Services);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
    "OTcwMkAzMjMwMkUzNDJFMzBPS3JpdmtTUjlQSmZldndUek5rRHdkSUFpaEtJc296dXdJM3pCdUhzNVpjPQ==");
builder.Services.AddSyncfusionBlazor(options => { options.EnableRtl = true; });
builder.Services.AddScoped<SfDialogService>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("API"));

//builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["HttpClient:BaseAddress"]!);
    client.Timeout = new TimeSpan(0, 0, 0, 30);
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
}); 

builder.Services.AddScoped<TokenProvider>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
        options =>
        {
            options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = builder.Configuration["IdentityServer:Authority"];
            options.ForwardSignIn=  builder.Configuration["IdentityServer:RedirectUri"];
            options.SignedOutRedirectUri = builder.Configuration["IdentityServer:PostLogoutRedirectUri"]; 
            options.ResponseType = "code";  
            options.ClientId = "Ticketing";
            options.ClientSecret = "Ticketing";
            options.Scope.Add("roles");
            options.Scope.Add("openid");
            options.Scope.Add("Ticketing.API");
            options.Scope.Add("offline_access");
            options.Scope.Add("Ticketing_Identity_Resource");
            options.UsePkce = true;
            options.SaveTokens = true;
            options.UseTokenLifetime = true;
            options.RequireHttpsMetadata = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                //map claim to name for display on the upper right corner after login.  Can be name, email, etc.
                NameClaimType = "name"
            };
        });


builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy  
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();