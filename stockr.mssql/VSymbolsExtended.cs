using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class VSymbolsExtended
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string PrimaryExchange { get; set; }
        public string Sector { get; set; }
        public DateTime DateModified { get; set; }
    }
}
