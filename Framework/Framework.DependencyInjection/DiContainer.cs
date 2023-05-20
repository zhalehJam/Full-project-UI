using System;
using Framework.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace framework.DependencyInjection
{
   public class DiContainer:IDIContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public DiContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public T Resolve<T>()
        {
          return  _serviceProvider.GetRequiredService<T>();
       
        }
    }
}
