using Framework.Core.UserDataManagement;
using Framework.UserDataManagement;
using Ticketing.Models.Centers.Repository;
using Ticketing.Models.Persons.Repository;
using Ticketing.Models.Programs.Repository;
using Ticketing.Models.Tickets.Repository;
using Ticketing.Repository.Centers;
using Ticketing.Repository.Persons;
using Ticketing.Repository.Programs;
using Ticketing.Repository.Tickets;

namespace Ticketing_UI
{
    public class Registrar //: RegistrarBase
    {
        public static void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            //serviceCollection.AddTransient<IHttpClientFactory>();
            serviceCollection.AddTransient<ICenterRepository, CenterRepository>();
            serviceCollection.AddTransient<IPersonRepository, PersonRepository>();
            serviceCollection.AddTransient<IProgramRepository, ProgramRepository>();
            serviceCollection.AddTransient<ITicketRepository, TicketRepository>();
            serviceCollection.AddScoped<IUserDataManagement, UserDataManagement>();

            var baseaddress = new Uri("https://localhost:44359/API/");

            //serviceCollection.AddHttpClient<ICenterRepository, CenterRepository>(client =>
            //{
            //    client.BaseAddress = baseaddress;
            //});
            //serviceCollection.AddHttpClient<IPersonRepository, PersonRepository>(client =>
            //{
            //    client.BaseAddress = baseaddress;
            //});
            //serviceCollection.AddHttpClient<IProgramRepository, ProgramRepository>(client =>
            //{
            //    client.BaseAddress = baseaddress;
            //});
            //serviceCollection.AddHttpClient<ITicketRepository, TicketRepository>(client =>
            //{
            //    client.BaseAddress = baseaddress;
            //});

        }
    }
}
