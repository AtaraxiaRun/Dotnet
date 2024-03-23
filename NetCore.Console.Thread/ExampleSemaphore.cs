using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    /// <summary>
    ///【跨进程，更复杂，更消耗性能】信号量Semaphore 允许指定多个线程访问资源，跨进程使用（比如访问本地操作系统某一个文件，或者锁定本地系统的打印机只能有一个进程访问），并且适用于更为复杂的同步需求
    /// </summary>
    class ExampleSemaphore
    {
        // 创建一个Semaphore实例，最大允许3个线程同时访问资源，初始也允许3个线程访问
        private static Semaphore _pool = new Semaphore(3, 3);

        static void Main()
        {
            for (int i = 1; i <= 5; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Worker));
                t.Start(i);
            }
        }

        static void Worker(object num)
        {
            Console.WriteLine($"Thread {num} is requesting the semaphore");
            _pool.WaitOne(); // 请求访问，代表占用了一个线程资源

            Console.WriteLine($"Thread {num} enters the semaphore");
            Thread.Sleep(1000); // 模拟工作

            Console.WriteLine($"Thread {num} is releasing the semaphore");
            _pool.Release(); // 释放访问资源
        }
    }
}
