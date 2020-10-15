using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class VQuoteLatest
    {
        public string Symbol { get; set; }
        public long? LatestVolume { get; set; }
        public decimal? Change { get; set; }
        public decimal? ChangePercent { get; set; }
        public long? LatestUpdate { get; set; }
        public decimal? LatestPrice { get; set; }
        public long? Rw { get; set; }
    }
}
