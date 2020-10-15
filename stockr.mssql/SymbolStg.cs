using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class SymbolStg
    {
        public int SymbolId { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public string IexId { get; set; }
        public string Region { get; set; }
        public string Currency { get; set; }
        public bool? IsEnabled { get; set; }
        public string Figi { get; set; }
        public string Cik { get; set; }
    }
}
