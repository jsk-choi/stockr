using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using db = stockr.mssql;

namespace stockr.service
{
    public class Processor : IProcessor
    {
        public async Task<IEnumerable<db.QuoteStg>> ConvertJsonToModel(IEnumerable<string> body)
        {
            var retval = new List<db.QuoteStg>();

            body = body.Select(x => JsonClean(x));

            foreach (var item in body)
            {
                try
                {
                    var quote = await Task.Run(() => JsonConvert.DeserializeObject<db.QuoteStg>(item));
                    retval.Add(quote);
                }
                catch (Exception ex)
                {
                    // TODO
                }
            }

            return retval;
        }

        private string JsonClean(string jsn)
        {
            return jsn.Replace("data: ", "").Replace("[{", "{").Replace("}]", "}");
        }
    }
}
