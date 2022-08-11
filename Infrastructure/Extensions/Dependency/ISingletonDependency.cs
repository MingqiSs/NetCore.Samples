﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions.Dependency
{
    public interface ISingletonDependency
    {
    }
    public interface ITransientDependency
    {

    }
    /// <summary>
    /// 依赖注册接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="listType">all type</param>
        /// <param name="config">Config</param>
        void Register(ContainerBuilder builder, List<Type> listType);

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
