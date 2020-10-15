using System;

using db = stockr.mssql;

namespace stockr.service
{
    public interface IDataProvider
    {
        void QuoteStagInsert(db.QuoteStg quoteStg);
    }
}
