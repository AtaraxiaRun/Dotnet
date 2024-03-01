using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.IOC
{
    public class Log4
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"Log4 ：{message}");
        }
    }
}
