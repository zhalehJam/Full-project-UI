using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Models.Centers.Repository;
using Ticketing.Models.Persons.Repository;
using Ticketing.Repository.Centers;
using Ticketing.Repository.Persons;
using TicketingUI;
using TicketingUI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient<ICenterRepository, CenterRepository>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44359/");
});
builder.Services.AddHttpClient<IPersonRepository, PersonRepository>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44359/");
});

//Registrar registrar = new Registrar(builder.Services);
//registrar.Register();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
