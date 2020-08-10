using System;
using Autofac;
using Quartz;
using Quartz.Spi;

namespace SmartFridgeApp.API.Outbox
{
    public class JobFactory : IJobFactory
    {
        private readonly IContainer _container;
        public JobFactory(IContainer container)
        {
            this._container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _container.Resolve(bundle.JobDetail.JobType);

            return job as IJob;
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
