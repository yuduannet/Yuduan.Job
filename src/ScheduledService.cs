using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace Yuduan.Quartz.DependencyInjection
{
    internal class ScheduledService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<ScheduledJob> _jobSchedules;

        public ScheduledService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<ScheduledJob> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        private IScheduler Scheduler { get; set; }

        public async Task StartAsync()
        {
            Scheduler = await _schedulerFactory.GetScheduler();
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                await ScheduleJob(jobSchedule);
            }

            await Scheduler.Start();
        }

        private async Task ScheduleJob(ScheduledJob scheduledJob)
        {
            var job = CreateJob(scheduledJob);
            var trigger = CreateTrigger(scheduledJob);

            await Scheduler.ScheduleJob(job, trigger);
        }

        public async Task StopAsync()
        {
            if (Scheduler != null) await Scheduler.Shutdown();
        }

        private IJobDetail CreateJob(ScheduledJob schedule)
        {
            var jobType = schedule.Type;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName ?? string.Empty)
                .WithDescription(jobType.Name)
                .Build();
        }

        private ITrigger CreateTrigger(ScheduledJob schedule)
        {
            var t = TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.Type.FullName}.trigger");
            if (schedule.ScheduleBuilder != null)
                t = t.WithSimpleSchedule(schedule.ScheduleBuilder);
            else if (!string.IsNullOrEmpty(schedule.CronExpression))
                t = t.WithCronSchedule(schedule.CronExpression);
            return t.WithDescription(schedule.Description ?? schedule.Type.FullName)
                 .Build();
        }
    }
}
