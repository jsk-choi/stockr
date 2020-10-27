using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using stockr.service;

public class Application
{
    public static void Main()
    {
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<App>().DooParallel("aaa fff").GetAwaiter().GetResult();
    }

    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        var config = LoadConfiguration();

        services.AddSingleton(config);
        services.AddTransient<ISvc_test, Svc_test>();
        services.AddTransient<IProcessor, Processor>();
        services.AddTransient<IDataProvider, DataProvider>();
        services.AddTransient<App>();

        return services;
    }

    public static IConfiguration LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("app.secret", optional: true, reloadOnChange: true);

        return builder.Build();
    }
}