using System;
using Quartz;

namespace Yuduan.Quartz.DependencyInjection
{
    internal sealed class ScheduledJob
    {
        public ScheduledJob(Type type, string cronExpression, string description = null)
        {
            Type = type;
            CronExpression = cronExpression;
            Description = description;
        }

        public ScheduledJob(Type type, Action<SimpleScheduleBuilder> scheduleBuilder, string description = null)
        {
            Type = type;
            ScheduleBuilder = scheduleBuilder;
            Description = description;
        }

        public Type Type { get; }

        public string CronExpression { get; }

        public string Description { get; }

        public Action<SimpleScheduleBuilder> ScheduleBuilder { get; set; }
    }
}
 