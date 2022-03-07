using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Yuduan.Quartz.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddJobs(
            this IServiceCollection services,
            Action<QuartzFactory> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            TryAddScheduledHostedService(services);
            setupAction(new QuartzFactory(services));
            return services;
        }

        public static void Add<TJob>(
            this QuartzFactory factory,
            string cronExpression,
            string description = null
          )
        {
            factory.Services.Add(new ServiceDescriptor(typeof(TJob), typeof(TJob), ServiceLifetime.Singleton));
            factory.Services.AddScoped(_ => new ScheduledJob(typeof(TJob), cronExpression, description));
        }
        public static void Add<TJob>(
            this QuartzFactory factory,
            Action<SimpleScheduleBuilder> setupAction,
            string description = null
        )
        {
            setupAction(SimpleScheduleBuilder.Create());
            factory.Services.Add(new ServiceDescriptor(typeof(TJob), typeof(TJob), ServiceLifetime.Singleton));
            factory.Services.AddTransient(_ => new ScheduledJob(typeof(TJob), setupAction, description));
        }
        private static void TryAddScheduledHostedService(this IServiceCollection services)
        {
            services.TryAddSingleton<IJobFactory, JobFactory>();
            services.TryAddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.TryAddSingleton<ScheduledService>();
        }
    }
}
