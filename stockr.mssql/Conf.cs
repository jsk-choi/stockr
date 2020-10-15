using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class Conf
    {
        public DateTime? LastUpdate { get; set; }
        public int? UpdateCount { get; set; }
    }
}
