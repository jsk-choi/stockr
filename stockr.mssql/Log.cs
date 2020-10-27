using System;
using System.Collections.Generic;

namespace stockr.mssql
{
    public partial class Log
    {
        public int Id { get; set; }
        public DateTime SystemTime { get; set; }
        public string Msg { get; set; }
        public string Catg { get; set; }
    }
}
