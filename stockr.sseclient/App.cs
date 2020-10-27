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
    private readonly ISvc_test _svc_Test;

    private readonly IConfiguration _config;
    private readonly IProcessor _processor;
    private readonly IDataProvider _dataProvider;

    private (DateTime tmp, IList<string> body) _body;

    public App(IConfiguration config, ISvc_test svc_Test, IProcessor processor, IDataProvider dataProvider)
    {
        _config = config;
        _svc_Test = svc_Test;
        _processor = processor;
        _dataProvider = dataProvider;

        _body.tmp = DateTime.Now;
        _body.body = new List<string>();
    }

    public async Task DooParallel(string durr = "") {
        try
        {
            Task t1 = Task.Run(() => ProcessOnTimer());
            Task t2 = Task.Run(() => Run(durr));

            await Task.WhenAll(t2, t1);
        }
        finally
        {
        }
    }

    private async Task Run(string jjj = "")
    {
        _svc_Test.DoTheThing(jjj);

        HttpClient client = new HttpClient();
        string url = $"https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols=ACCD,ADM,AFG,AGO,AINV,ALIM,AMAL,AMRN,AOA,AQB,AROC,ASTE,ATXI,AWI,BAC-B,BBK,BDCX,BGB,BIOL,BLCT,BNDW,BPY,BSBK,BSX-A,BXS,CANG,CBT,CDEV,CETX,CHCO,CHSCO,CL,CLXT,CNF,COLB,CPRI,CRSAW,CTBB,CVCY,CYCN,DBS&token=pk_6936d6bbead54838ab45b0f845ece345";
        int msgSize = _config.GetValue<int>("AppConfig:MessageSizeMin");
        int batchSize = _config.GetValue<int>("AppConfig:QuoteBatchSize");

        while (true)
        {
            try
            {
                using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var message = await streamReader.ReadLineAsync();

                        if (message.Contains("data: ") && message.Length > msgSize) {
                            _body.tmp = DateTime.Now;
                            _body.body.Add(message);
                        }

                        if (_body.body.Count == batchSize) {
                            await SendToProcess();
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                _dataProvider.Log(ex.Message, "err");
            }
        }
    }

    private async Task SendToProcess()
    {
        var sendbody = _body.body.ToList();
        _body.tmp = DateTime.Now;
        _body.body.Clear();

        await ProcessQuotes(sendbody);
    }

    private async Task ProcessOnTimer()
    {
        var interv = _config.GetValue<int>("AppConfig:ProcessOnTimerInterval");
        
        while (true) {

            var interval = interv * 60 * 1000;

            //Thread.Sleep(interval);
            Thread.Sleep(6000);

            _dataProvider.Log($"On ProcessOnTimer", "info");

            if ((DateTime.Now - _body.tmp).TotalMilliseconds > interval && _body.body.Count() > 0) {
                await SendToProcess();
            }
        }
    }
    private async Task ProcessQuotes(List<string> sendbody)
    {
        _dataProvider.Log($"Loaded ct : {sendbody.Count()}", "info");
        var quotestag = await _processor.ConvertJsonToModel(sendbody);
        _dataProvider.QuoteStagInsert(quotestag);
    }
}