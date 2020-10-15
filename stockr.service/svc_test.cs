using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using db = stockr.mssql;

namespace stockr.service
{
    public class Svc_test : ISvc_test
    {
        public void DoTheThing(string durr)
        {
            Console.WriteLine("Svc_test: " + durr);


            using (var ctx = new db.stockrContext())
            {
                var jjj = ctx.Symbol.OrderBy(x => Guid.NewGuid()).Select(x => x.Symbol1).Take(20);

                foreach (var item in jjj)
                {
                    Console.WriteLine("Svc_test: " + item);
                }
            };
        }
    }
}
