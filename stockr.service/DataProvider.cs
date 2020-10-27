using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using db = stockr.mssql;

namespace stockr.service
{
    public class DataProvider : IDataProvider
    {
        private int logCt { get; set; }

        public DataProvider() {
            logCt = 1;
        }

        public void QuoteStagInsert(db.QuoteStg quoteStg)
        {
            try
            {
                using (var ctx = new db.StockrContext())
                {
                    ctx.QuoteStg.Add(quoteStg);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, "err");
            }
        }
        public void QuoteStagInsert(IEnumerable<db.QuoteStg> quoteStg)
        {
            try
            {
                using (var ctx = new db.StockrContext())
                {
                    ctx.QuoteStg.AddRange(quoteStg);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, "err");
            }
        }

        public void Log(string msg, string catg = "") {

            var outtxt = $"{(logCt++).ToString("D6")}\tc:{catg}\t{msg}";
            Console.WriteLine(outtxt);

            try
            {
                using (var ctx = new db.StockrContext())
                {
                    ctx.Log.Add(new db.Log { 
                        Catg = catg,
                        Msg = msg
                    });
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
