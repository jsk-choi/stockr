using System;
using System.Collections.Generic;

using db = stockr.mssql;

namespace stockr.service
{
    public interface IDataProvider
    {
        void QuoteStagInsert(db.QuoteStg quoteStg);
        void QuoteStagInsert(IEnumerable<db.QuoteStg> quoteStg);
        void Log(string msg, string catg);
    }
}
