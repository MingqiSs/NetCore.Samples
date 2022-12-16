using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Infrastructure.Extensions.AutofacManager
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, List<Type> listType);
    }
}
