using Autofac;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SmartFridgeApp.API.Quartz;

namespace SmartFridgeApp.API.Configuration.Modules
{
    public class QuartzModule : Autofac.Module
    {
        private readonly string EveryHalfHour = "* 0/30 * * * ?";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JobFactory>()
                .As<IJobFactory>()
                .SingleInstance();

            builder.RegisterType<StdSchedulerFactory>()
                .As<ISchedulerFactory>()
                .SingleInstance();

            builder.RegisterType<QuartzJobRunner>()
                .SingleInstance();

            builder.RegisterType<ProcessOutboxJob>()
                .SingleInstance();

            builder.Register(c => new JobSchedule(jobType: typeof(ProcessOutboxJob), cronExpression: EveryHalfHour))
                .SingleInstance();
        }
    }
}
