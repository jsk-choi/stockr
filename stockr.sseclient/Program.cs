using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using stockr.service;

public class Application
{
    //static void Main(string[] args)
    //{
    //    MainAsync(args).GetAwaiter().GetResult();
    //}

    //static async Task MainAsync(string[] args)
    //{
    //    HttpClient client = new HttpClient();
    //    client.Timeout = TimeSpan.FromSeconds(5);
    //    string url = $"https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols=ACCD,ADM,AFG,AGO,AINV,ALIM,AMAL,AMRN,AOA,AQB,AROC,ASTE,ATXI,AWI,BAC-B,BBK,BDCX,BGB,BIOL,BLCT,BNDW,BPY,BSBK,BSX-A,BXS,CANG,CBT,CDEV,CETX,CHCO,CHSCO,CL,CLXT,CNF,COLB,CPRI,CRSAW,CTBB,CVCY,CYCN,DBS&token=pk_6936d6bbead54838ab45b0f845ece345";

    //    while (true)
    //    {
    //        try
    //        {
    //            Console.WriteLine("Establishing connection");
    //            using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
    //            {
    //                while (!streamReader.EndOfStream)
    //                {
    //                    var message = await streamReader.ReadLineAsync();
    //                    Console.WriteLine($"Received price update: {message}");
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Here you can check for 
    //            //specific types of errors before continuing
    //            //Since this is a simple example, i'm always going to retry
    //            Console.WriteLine($"Error: {ex.Message}");
    //            Console.WriteLine("Retrying in 5 seconds");
    //            await Task.Delay(TimeSpan.FromSeconds(5));
    //        }
    //    }
    //}

    public static void Main()
    {
        var services = ConfigureServices();

        var serviceProvider = services.BuildServiceProvider();

        // calls the Run method in App, which is replacing Main
        serviceProvider.GetService<App>().Run().GetAwaiter().GetResult();
        //serviceProvider.GetService<App>().dooParallel();
    }

    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        var config = LoadConfiguration();
        services.AddSingleton(config);

        services.AddTransient<ISvc_test, Svc_test>();
        services.AddTransient<IProcessor, Processor>();
        
        // required to run the application
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