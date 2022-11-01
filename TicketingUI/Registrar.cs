using Framework.DependencyInjection;
using Ticketing.Models.Centers.Repository;
using Ticketing.Models.Persons.Repository;
using Ticketing.Repository.Centers;

namespace TicketingUI
{
    public class Registrar : RegistrarBase
    {
        public Registrar(IServiceCollection serviceCollection) : base(serviceCollection)
        {
        }

        public override void Register()
        {
            serviceCollection.AddHttpContextAccessor(); 
            serviceCollection.AddHttpClient<IHttpClientFactory>();
            serviceCollection.AddTransient<ICenterRepository, CenterRepository>();
            serviceCollection.AddTransient<IPersonRepository, IPersonRepository>();
        }
    }
}
