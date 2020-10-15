using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class MarketOpen
    {
        public int Id { get; set; }
        public DateTime DtOpen { get; set; }
        public DateTime DtClose { get; set; }
        public DateTime DateModified { get; set; }
    }
}
