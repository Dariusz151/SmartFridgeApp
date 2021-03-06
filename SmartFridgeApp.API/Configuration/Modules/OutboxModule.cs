﻿using System.Linq;
using System.Reflection;
using Autofac;
using Quartz;
using Module = Autofac.Module;

namespace SmartFridgeApp.API.Configuration.Modules
{
    public class OutboxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => typeof(IJob).IsAssignableFrom(x)).InstancePerDependency();
        }
    }
}
