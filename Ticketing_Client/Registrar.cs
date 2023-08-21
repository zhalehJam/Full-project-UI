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

namespace Ticketing_Client
{
    public class Registrar //: RegistrarBase
    {
        public static void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddTransient<ICenterRepository, CenterRepository>();
            serviceCollection.AddTransient<IPersonRepository, PersonRepository>();
            serviceCollection.AddTransient<IProgramRepository, ProgramRepository>();
            serviceCollection.AddTransient<ITicketRepository, TicketRepository>();
            serviceCollection.AddScoped<IUserDataManagement, UserDataManagement>();

        }
    }

}


