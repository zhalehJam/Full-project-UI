using Framework.AssemblyHelper;
using Framework.Core.AssemblyHelper;
using Framework.Core.DependencyInjection;
using Framework.Core.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace framework.DependencyInjection
{
    public abstract class RegistrarBase<TRegistrar> : IRegistrar
   {
       private  IServiceCollection _serviceCollection;
       private IAssemblyDiscovery _assemblyDiscovery;
       private readonly string _namespace;
        protected RegistrarBase()
        {
     
            var nameSpaceSpell = typeof(TRegistrar).Namespace?.Split('.');
            _namespace = nameSpaceSpell?[0] + "." + nameSpaceSpell?[1];
        }
       public void Register(IServiceCollection serviceCollection, IAssemblyDiscovery assemblyDiscovery)
       {
           _serviceCollection = serviceCollection;
           _assemblyDiscovery = assemblyDiscovery;
           RegisterFramework();
           RegisterTransient<IRepository>();
           
            RegisterTransientHttpClient();
        }

       private void RegisterTransientHttpClient()
       {
           _serviceCollection.AddTransient<HttpClient>(provider =>
           {
               HttpClient client = new HttpClient();
               client.Timeout = new TimeSpan(0, 0, 0, 30);
               return client;
           });
       }

       private void RegisterFramework()
       {
           _serviceCollection.AddScoped<IAssemblyDiscovery, AssemblyDiscovery>(a => new AssemblyDiscovery("GTS*.dll"));
        }
        private void RegisterTransient<TRegisterBaseType>()
       {
           var types = _assemblyDiscovery.DiscoverTypes<TRegisterBaseType>(_namespace);
           foreach (var type in types)
           {
               var baseInterface = type.GetInterfaces()
                   .First(a => a.Name != typeof(TRegisterBaseType).Name);
               _serviceCollection.AddTransient(baseInterface, type);
           }
       }
       private void RegisterScope<TRegisterBaseType>()
       {
           var types = _assemblyDiscovery.DiscoverTypes<TRegisterBaseType>(_namespace);
           foreach (var type in types)
           {
               var baseInterface = type.GetInterfaces()
                   .First(a => a.Name != typeof(TRegisterBaseType).Name);
               _serviceCollection.AddScoped(baseInterface, type);
           }
       }
       private void RegisterSingleton<TRegisterBaseType>()
       {
           var types = _assemblyDiscovery.DiscoverTypes<TRegisterBaseType>(_namespace);
           foreach (var type in types)
           {
               var baseInterface = type.GetInterfaces()
                   .First(a => a.Name != typeof(TRegisterBaseType).Name);
               _serviceCollection.AddSingleton(baseInterface, type);
           }
       }
   }
}
