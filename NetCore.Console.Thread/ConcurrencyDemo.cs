using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    #region 并发编程
    /// <summary>
    /// 并发编程
    /// </summary>
    public class ConcurrencyDemo
    {
        private int[] data; //实际并发中使用的数据集合
        private List<Task> tasks = new List<Task>();

        public ConcurrencyDemo(int size)
        {
            data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i;
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public void ExecuteTasks()
        {
            //使用Tread处理数组
            //优点：1.简洁
            //缺点：1.无法对即时对返回值做处理 2.没有异常处理 3.不能接着with后续任务 4.不能异步 5.不能中途取消
            //场景：简单的并发需求可以使用，不需要对返回值做更新，不需要异常处理，不需要中途取消
            Console.WriteLine("**************使用Tread处理数组********");
            // 将线程列表存储在一个变量中，以便之后可以加入这些线程
            List<Thread> threads = new List<Thread>();
            foreach (int item in data)
            {
                var thread = new Thread(() => SafeDoWork(item));
                threads.Add(thread); // 将线程添加到列表中
                thread.Start();
                Thread.Sleep(500); //等待1秒

            }
            // 等待所有使用Thread创建的线程完成
            foreach (var thread in threads)
            {
                thread.Join(); // 等待线程完成
            }
            //使用Task.Run处理数组
            //优点：1.性能好 2.异步  3.异常处理 4.可以接着with后续任务
            //缺点：
            //场景：使用 Run 来轻松启动针对线程池的计算密集型任务。 这是用于启动一项计算密集型任务的首选机制 。
            Console.WriteLine("**************使用Task.Run处理数组********");
            foreach (int item in data)
            {
                var runTask = Task.Run(() => SafeDoWork(item));
                tasks.Add(runTask);
                Thread.Sleep(500); //等待1秒

            }

            // 使用Task.Factory.StartNew处理数组
            //优点：1.性能好 2.异步  3.异常处理 4.可以接着with后续任务  5.【与task.run相比】 StartNew方法重载版本有17个，对Task的控制更加强一些
            //缺点：
            //场景：需要对任务有更好控制的时候，比如完成后要执行什么，更好的异常处理你可以。  

            Console.WriteLine("**************使用Task.Factory.StartNew处理数组********");
            foreach (var item in data)
            {
                var factoryTask = Task.Factory.StartNew(() => SafeDoWork(item), TaskCreationOptions.LongRunning);
                tasks.Add(factoryTask);
                Thread.Sleep(500); //等待1秒
            }

            // 使用基础的Task
            // 优点：
            // 缺点：
            // 场景：希望分别生成并计划任务时，请使用 Task 类型的构造函数和 Start 方法。 公共方法必须仅返回已开始的任务
            Console.WriteLine("**************使用Task.Start处理数组********");
            foreach (var item in data)
            {
                var task=new Task(() => SafeDoWork(item));
                tasks.Add(task);
                Thread.Sleep(500); //等待1秒
            }
        }

        private void SafeDoWork(int item)
        {
            try
            {
                DoWork(item);
            }
            catch (Exception ex)
            {
                // 异常处理逻辑：记录异常信息到日志
                Console.WriteLine($"处理元素 {item} 时发生异常: {ex.Message}");
            }
        }

        private void DoWork(int item)
        {
            // 模拟一些计算，例如：计算数组元素的平凡
            int result = item * item;
            // 模拟工作正在进行
            Thread.Sleep(1000); // 等待1秒
            Console.WriteLine($"元素 {item} 的平方是 {result}，由线程 {Thread.CurrentThread.ManagedThreadId} 处理。");

        }

        public void WaitForAllTasks()
        {
            try
            {
                // 等待所有Task完成
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    Console.WriteLine(ex.Message);
                    return true;// 表示异常已经处理
                });


            }
        }

    }
    #endregion
}
