using Framework.Core.UserDataManagement;
using Framework.DependencyInjection;
using Framework.UserDataManagement;
using Ticketing.Models.Centers.Repository;
using Ticketing.Models.Persons.Repository;
using Ticketing.Models.Programs.Repository;
using Ticketing.Models.Tickets.Repository;
using Ticketing.Repository.Centers;
using Ticketing.Repository.Persons;
using Ticketing.Repository.Programs;
using Ticketing.Repository.Tickets;

namespace TicketingUI
{
    public class Registrar : RegistrarBase
    {
        public Registrar(IServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        public override void Register()
        {
            //    serviceCollection.AddHttpContextAccessor(); 
            //    serviceCollection.AddHttpClient<IHttpClientFactory>();
            //    serviceCollection.AddTransient<ICenterRepository, CenterRepository>();
            //    serviceCollection.AddTransient<IPersonRepository, IPersonRepository>();
            var baseaddress = new Uri("https://localhost:44359/API/");

            serviceCollection.AddHttpClient<ICenterRepository, CenterRepository>(client =>
            {
                client.BaseAddress = baseaddress;
            });
            serviceCollection.AddHttpClient<IPersonRepository, PersonRepository>(client =>
            {
                client.BaseAddress = baseaddress;
            });
            serviceCollection.AddHttpClient<IProgramRepository, ProgramRepository>(client =>
            {
                client.BaseAddress = baseaddress;
            });
            serviceCollection.AddHttpClient<ITicketRepository, TicketRepository>(client =>
            {
                client.BaseAddress = baseaddress;
            });
            serviceCollection.AddScoped<IUserDataManagement, UserDataManagement>();

        }
    }
}
