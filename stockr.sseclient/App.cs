using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

using stockr.service;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Configuration.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class App
{
    private readonly IConfiguration _config;
    private ISvc_test _svc_Test;

    private IList<string> _body;

    public App(IConfiguration config, ISvc_test svc_Test)
    {
        _config = config;
        _svc_Test = svc_Test;

        _body = new List<string>();
    }

    public async Task Run()
    {
        _svc_Test.DoTheThing("durrtho");

        //Console.Write("Attempting to open stream\n");

        //var url_quote_sse = "https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols=ACCD,ADM,AFG,AGO,AINV,ALIM,AMAL,AMRN,AOA,AQB,AROC,ASTE,ATXI,AWI,BAC-B,BBK,BDCX,BGB,BIOL,BLCT,BNDW,BPY,BSBK,BSX-A,BXS,CANG,CBT,CDEV,CETX,CHCO,CHSCO,CL,CLXT,CNF,COLB,CPRI,CRSAW,CTBB,CVCY,CYCN,DBS&token=pk_6936d6bbead54838ab45b0f845ece345";

        //var response = OpenSSEStream(url_quote_sse);

        //Console.Write("Success! \n");


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
                        Console.WriteLine($"Received price update: {message}");
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


    private Stream OpenSSEStream(string url)
    {
        var request = WebRequest.Create(new Uri(url));
        ((HttpWebRequest)request).AllowReadStreamBuffering = false;
        var response = request.GetResponse();
        var stream = response.GetResponseStream();

        ReadStreamForever(stream);

        return stream;
    }

    private void ReadStreamForever(Stream stream)
    {
        var encoder = new UTF8Encoding();
        var buffer = new byte[16777216];
        while (true)
        {
            //TODO: Better evented handling of the response stream

            if (stream.CanRead)
            {
                int len = stream.Read(buffer, 0, buffer.Length);
                if (len > 0)
                {
                    var text = encoder.GetString(buffer, 0, len);

                    var lines = text.Trim().Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        _body.Add(lines[i]);
                    }
                }
            }
            //System.Threading.Thread.Sleep(250);
        }
    }
}