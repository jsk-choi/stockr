using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using db = stockr.mssql;

namespace stockr.service
{
    public class DataProvider : IDataProvider
    {
        public void QuoteStagInsert(db.QuoteStg quoteStg)
        {
            using (var ctx = new db.stockrContext()) {
                ctx.QuoteStg.Add(quoteStg);
            }
        }
    }
}
