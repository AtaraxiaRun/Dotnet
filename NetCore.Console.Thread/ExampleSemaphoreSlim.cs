using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    /// <summary>
    /// 【不跨进程】信号量 SemaphoreSlim用于限制资源的并发访问，用于同一进程内的线程同步和异步任务编程
    /// </summary>
    public class ExampleSemaphoreSlim
    {
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(3); //只允许同时有3个线程

        public static async Task Main()
        {
            var tasks = new Task[5];

            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Run(() => AccessDatabaseAsync(i));
            }

            await Task.WhenAll(tasks);
        }

        private static async Task AccessDatabaseAsync(int id)
        {
            await _semaphoreSlim.WaitAsync();
            Console.WriteLine($"Task {id} is accessing the database.");
            // 模拟异步工作。
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine($"Task {id} is releasing the semaphore.");
            _semaphoreSlim.Release();
        }
    }
}
