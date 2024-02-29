using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.IOC
{
    public interface ILog2
    {
        void WriteLog(string Message);
    }
    public class Log2 : ILog2
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"Log2 ：{message}");
        }
    }
}
