using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using stockr.service;


public class App
{
    private readonly IConfiguration _config;
    private ISvc_test _svc_Test;
    private IProcessor _processor;

    private IList<string> _body;

    private int ctr;
    private int ct;
    private List<string> vals;

    public App(IConfiguration config, ISvc_test svc_Test, IProcessor processor)
    {
        _config = config;
        _svc_Test = svc_Test;
        _processor = processor;

        _body = new List<string>();

        vals = new List<string>();

        ct = 0;
        ctr = 0;
    }

    public void dooParallel() {
        Parallel.Invoke(
            () => firstOne(),
            () => SecondOne());
    }

    private void firstOne() {
        do
        {
            vals.Add((++ct).ToString("D6"));
            Thread.Sleep(638);
        } while (true);
    }
    private void SecondOne() {
        do
        {
            Thread.Sleep(4000);
            var pvals = vals;
            vals = new List<string>();

            Console.WriteLine($"series {(++ctr).ToString("D4")} : {string.Join(',', pvals)}");

        } while (true);
    }

    public async Task Run()
    {
        int ctn = 0;
        _svc_Test.DoTheThing("durrtho");

        HttpClient client = new HttpClient();
        string url = $"https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols=ACCD,ADM,AFG,AGO,AINV,ALIM,AMAL,AMRN,AOA,AQB,AROC,ASTE,ATXI,AWI,BAC-B,BBK,BDCX,BGB,BIOL,BLCT,BNDW,BPY,BSBK,BSX-A,BXS,CANG,CBT,CDEV,CETX,CHCO,CHSCO,CL,CLXT,CNF,COLB,CPRI,CRSAW,CTBB,CVCY,CYCN,DBS&token=pk_6936d6bbead54838ab45b0f845ece345";

        while (true)
        {
            try
            {
                using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var message = await streamReader.ReadLineAsync();

                        if (message.Contains("data: "))
                            _body.Add(message);

                        if (_body.Count == 10) {

                            var sendbody = _body.ToList();
                            _body.Clear();

                            Parallel.Invoke(() => ProcessQuotes(sendbody));
                            
                        }
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
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }

    private void ProcessQuotes(List<string> sendbody)
    {
        var quotestag = _processor.ConvertJsonToModel(sendbody).GetAwaiter().GetResult();
    }
}