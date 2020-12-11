using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using db = stockr.mssql;

namespace stockr.service
{
    public interface IDataProvider
    {
        string srcid { get; set; }
        void QuoteStagInsert(db.QuoteStg quoteStg);
        void QuoteStagInsert(IEnumerable<db.QuoteStg> quoteStg);
        IList<db.VSymbols> SymbolsGet();
        //IList<db.VSymbols> Jfff(Expression<Func<db.VSymbols, db.VSymbols>> select);
        void Log(string msg, string catg);
    }
}
