using Microsoft.Extensions.DependencyInjection;

namespace Yuduan.Quartz.DependencyInjection
{
    public class QuartzFactory
    {
        public IServiceCollection Services { get; }

        public QuartzFactory(IServiceCollection services)
        {
            Services = services;
        }
    }
}
