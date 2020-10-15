using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using stockr.service;


public class App
{
    private readonly IConfiguration _config;
    private ISvc_test _svc_Test;

    private IList<string> _body;

    private int ctOne;
    private int ctTwo;

    public App(IConfiguration config, ISvc_test svc_Test)
    {
        _config = config;
        _svc_Test = svc_Test;

        _body = new List<string>();
    }

    public void dooParallel() {
        Parallel.Invoke(
            () => firstOne(),
            () => SecondOne());
    }

    private void firstOne() {
        var ct = 0;
        do
        {
            Console.WriteLine($"from first ct : {(++ctOne).ToString("D4")} - second     ct : {(ctTwo).ToString("D4")}");
            Thread.Sleep(578);
        } while (true);
    }
    private void SecondOne() {
        var ct = 0;
        do
        {
            Console.WriteLine($"second     ct : {(++ctTwo).ToString("D4")} - from first ct : {(ctOne).ToString("D4")}");
            Thread.Sleep(Convert.ToInt16(578.0 * 1.4372));
        } while (true);
    }

    private async Task Run()
    {
        int ctn = 0;
        _svc_Test.DoTheThing("durrtho");

        HttpClient client = new HttpClient();
        string url = $"https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols=ACCD,ADM,AFG,AGO,AINV,ALIM,AMAL,AMRN,AOA,AQB,AROC,ASTE,ATXI,AWI,BAC-B,BBK,BDCX,BGB,BIOL,BLCT,BNDW,BPY,BSBK,BSX-A,BXS,CANG,CBT,CDEV,CETX,CHCO,CHSCO,CL,CLXT,CNF,COLB,CPRI,CRSAW,CTBB,CVCY,CYCN,DBS&token=pk_6936d6bbead54838ab45b0f845ece345";

        while (true)
        {
            try
            {
                Console.WriteLine("Establishing connection");
                using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var message = await streamReader.ReadLineAsync();
                        Console.WriteLine((++ctn).ToString("D6"));
                        Console.WriteLine(message);
                    }
                }
            }
            catch (Exception ex)
            {
                //Here you can check for 
                //specific types of errors before continuing
                //Since this is a simple example, i'm always going to retry
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Retrying in 2 seconds");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }

}