using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Wiki.Infrastructure.IOC.Modules;
using Wiki.Infrastructure.Mappers;

namespace Wiki.Infrastructure.IOC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration configuration;

        public ContainerModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder )
        {
            builder.RegisterInstance(AutoMapperConfig.Initialize()).SingleInstance();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<RepositoryModule>();
        }
    }
}
