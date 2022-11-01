using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DependencyInjection
{
    public abstract class RegistrarBase : IRegistrar
    {
        protected readonly IServiceCollection serviceCollection;

        protected RegistrarBase(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
            RegisterDiContainer();
        }
        public abstract void Register();

        private void RegisterDiContainer()
        {
            serviceCollection.AddTransient<IDiContainer, DiContainer>();
        }
    }
}
