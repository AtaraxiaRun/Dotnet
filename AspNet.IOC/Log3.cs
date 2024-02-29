using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.IOC
{
    public interface ILog3
    {
        void WriteLog(string Message);
    }
    public class Log3 : ILog3
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"Log3 ：{message}");
        }
    }
}
