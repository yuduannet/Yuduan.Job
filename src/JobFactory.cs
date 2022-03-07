using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Yuduan.Quartz.DependencyInjection
{
    internal class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _service;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _service = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _service.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job) { }

    }
}
