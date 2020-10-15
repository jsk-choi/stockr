using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class VSymbols
    {
        public int SymbolId { get; set; }
        public string IexId { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public bool? IsEnabled { get; set; }
        public string Type { get; set; }
        public DateTime? Createdate { get; set; }
        public long? Rw { get; set; }
    }
}
