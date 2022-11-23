using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Models.Centers.Repository;
using Ticketing.Models.Persons.Repository;
using Ticketing.Models.Programs.Repository;
using Ticketing.Models.Tickets.Repository;
using Ticketing.Repository.Centers;
using Ticketing.Repository.Persons;
using Ticketing.Repository.Programs;
using Ticketing.Repository.Tickets;
using TicketingUI;
using TicketingUI.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddBlazoredModal();


        Registrar registrar = new Registrar(builder.Services);
        registrar.Register();

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
    }
}