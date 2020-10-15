using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class SymbolsBak
    {
        public int Id { get; set; }
        public int IexId { get; set; }
        public string Symbol { get; set; }
        public string SymbolName { get; set; }
        public bool? IsEnabled { get; set; }
        public string SymbolType { get; set; }
        public DateTime DateModified { get; set; }
    }
}
