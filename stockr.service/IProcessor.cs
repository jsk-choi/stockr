using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using db = stockr.mssql;

namespace stockr.service
{
    public interface IProcessor
    {
        Task<IEnumerable<db.QuoteStg>> ConvertJsonToModel(IEnumerable<string> body);
    }
}
