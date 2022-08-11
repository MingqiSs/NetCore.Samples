using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AutofacManager
{
    public static class AutofacContainerModule
    {
        public static ILifetimeScope container { get; set; }

        public static TService Resolve<TService>() where TService : class
        {
            if (container == null)
            {
                throw new ArgumentNullException($"AutofacContainerModule Resolve is NULL");
            }
            return container.Resolve<TService>();
        }
        public static TService GetService<TService>() where TService : class
        {
            return typeof(TService).GetService() as TService;
        }
        private static object GetService(this Type serviceType)
        {
            if (Utilities.HttpContext.Current != null)
            {
                return Utilities.HttpContext.Current.RequestServices.GetService(serviceType);
            }
            else if (ServiceProviderAccessor.ServiceProvider != null)
            {
                return ServiceProviderAccessor.ServiceProvider.GetService(serviceType);
            }
            throw new ArgumentNullException($"AutofacContainerModule GetService is NULL");
        }
    }

}
