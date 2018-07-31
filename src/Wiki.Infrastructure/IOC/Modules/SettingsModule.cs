using System;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Wiki.Infrastructure.Extensions;
using Wiki.Infrastructure.Settings;

namespace Wiki.Infrastructure.IOC.Modules
{
    public class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration configuration;

        public SettingsModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(configuration.GetSettings<SqlSettings>()).SingleInstance();
        }

    }
}
