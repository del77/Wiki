using System;
using System.Reflection;
using Autofac;
using Wiki.Infrastructure.Services;

namespace Wiki.Infrastructure.IOC.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ServiceModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            // builder.RegisterType<Encrypter>()
            //        .As<IEncrypter>()
            //        .SingleInstance();

            // builder.RegisterType<JwtHandler>()
            //        .As<IJwtHandler>()
            //        .SingleInstance();
        }
    }
}
