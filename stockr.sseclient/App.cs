using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using stockr.service;
using db = stockr.mssql;


public class App
{
    private readonly ISvc_test _svc_Test;

    private readonly IConfiguration _config;
    private readonly IProcessor _processor;
    private IDataProvider _dataProvider;

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



    public async Task DooParallel(string sseurl, string srcid) {

        //var symbols = _dataProvider.SymbolsGet().Select(x => x.Symbol);
        //var lst_sym = new List<string>();
        //var chunksize = 45;

        //while (symbols.Any())
        //{
        //    var sym_csv = string.Join(',', symbols.Take(chunksize));
        //    lst_sym.Add(sym_csv);            
        //    symbols = symbols.Skip(chunksize);
        //}


        //foreach (var item in lst_sym)
        //{
        //    var syms = item.Split(',');
        //    var src = $"{syms.First()}_{syms.Last()}";

        //    var quote_sse_url = $@"https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?token=pk_6936d6bbead54838ab45b0f845ece345&symbols={item}";
        //    var cmd = $@"START /B dotnet stockr.sseclient.dll ""{quote_sse_url}"" {src} > {src}.log";

        //    Console.WriteLine(cmd);
        //}

        try
        {
            Task t1 = Task.Run(() => ProcessOnTimer());
            Task t2 = Task.Run(() => Run(sseurl, srcid));

            await Task.WhenAll(t2, t1);
        }
        finally
        {
        }
    }

    private async Task Run(string sseurl, string srcid)
    {
        _svc_Test.DoTheThing(srcid);
        _dataProvider.srcid = srcid;

        HttpClient client = new HttpClient();
        //string url = $"https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols=ACCD,ADM,AFG,AGO,AINV,ALIM,AMAL,AMRN,AOA,AQB,AROC,ASTE,ATXI,AWI,BAC-B,BBK,BDCX,BGB,BIOL,BLCT,BNDW,BPY,BSBK,BSX-A,BXS,CANG,CBT,CDEV,CETX,CHCO,CHSCO,CL,CLXT,CNF,COLB,CPRI,CRSAW,CTBB,CVCY,CYCN,DBS&token=pk_6936d6bbead54838ab45b0f845ece345";
        string url = sseurl;
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
        var interv = _config.GetValue<decimal>("AppConfig:ProcessOnTimerInterval");
        
        while (true) {

            var interval = Convert.ToInt32(interv * 60 * 1000);

            Thread.Sleep(interval);
            //Thread.Sleep(6000);

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
