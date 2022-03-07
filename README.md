# Yuduan.Quartz.DependencyInjection

use Microsoft.Extensions.DependencyInjection to register [Quartz.Net](https://github.com/quartznet/quartznet). 使用微软的DI注册Job

Supports

 * .Net Standard 2.0 or above
 * .Net Framework 4.6.1 or above

[![NuGet version (Yuduan.Quartz.DependencyInjection)](https://img.shields.io/nuget/v/Yuduan.Quartz.DependencyInjection.svg?style=flat-square)](https://www.nuget.org/packages/Yuduan.Quartz.DependencyInjection/)

Install by [nuget](https://www.nuget.org/packages/Yuduan.Quartz.DependencyInjection)

    Install-Package Yuduan.Quartz.DependencyInjection

### Simple Usage
```csharp
    private static readonly IServiceCollection Services = new ServiceCollection();
    static async Task Main(string[] args)
    {
        Services.AddJobs(c =>
        {
            c.Add<JobA>(s => s.WithIntervalInMinutes(30).RepeatForever());
            //register with cron
            c.Add<JobAB>("0 0 10/1 * * ? *");
        }
    }
   
```
