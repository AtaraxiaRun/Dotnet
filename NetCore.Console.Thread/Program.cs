using System.Collections.Concurrent;

namespace NetCore.ConsoleThread
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if false
            #region 并发编程
            ConcurrencyDemo demo = new ConcurrencyDemo(3);
            demo.ExecuteTasks();

            // 等待所有任务完成，这是更加安全的退出方式
            demo.WaitForAllTasks();
            //程序完成所有处理后退出
            Console.WriteLine("并发编程 所有任务完成,程序退出");

            /*
             输出：
            **************使用Tread处理数组********
            元素 0 的平方是 0，由线程 12 处理。
            元素 1 的平方是 1，由线程 13 处理。
            元素 2 的平方是 4，由线程 14 处理。
            **************使用Task.Run处理数组********
            元素 0 的平方是 0，由线程 11 处理。
            元素 1 的平方是 1，由线程 7 处理。  ，这里没有输出三条数据，是因为Task.Run与Task.Factory.StartNew是并发异步执行的，所以可能执行顺序不一样
            **************使用Task.Factory.StartNew处理数组********
            元素 2 的平方是 4，由线程 11 处理。   
            元素 0 的平方是 0，由线程 15 处理。
            元素 1 的平方是 1，由线程 16 处理。
            元素 2 的平方是 4，由线程 17 处理。
            所有任务完成,程序退出
             */
            #endregion
            #region 并行编程
            ParallelismDemo parallelismDemo = new ParallelismDemo(3);

            // 创建 CancellationTokenSource 以便在需要时取消操作
            CancellationTokenSource cts = new CancellationTokenSource();
            //cts.Cancel(); //如果需要取消,则取消注释

            parallelismDemo.ProcessDataInParallel(cts.Token);
            /*
             输出：
            ************使用Parallel.For 进行并行处理************
            元素 0 的处理结果是 0，由线程 7 处理。
            元素 2 的处理结果是 4，由线程 18 处理。
            元素 1 的处理结果是 1，由线程 1 处理。
            ************使用Parallel.ForEach 对数组进行并行迭代************
            元素 0 的处理结果是 0，由线程 1 处理。
            元素 1 的处理结果是 1，由线程 18 处理。
            元素 2 的处理结果是 4，由线程 7 处理。
            ************使用Parallel.ForEach 和 Partitioner 进行数据处理************
            元素 0 的处理结果是 0，由线程 19 处理。
            元素 2 的处理结果是 4，由线程 21 处理。
            元素 1 的处理结果是 1，由线程 5 处理。
             */
            Console.WriteLine("(并行任务会阻塞主线程)并行编程 所有任务完成,程序退出");
            #endregion
            #region 异步任务
            var asyncDemo = new AsyncDemo();

            // 创建 CancellationTokenSource 以便在需要时取消操作
            CancellationTokenSource asyncCts = new CancellationTokenSource();

            // 设置超时时间为 10 秒
            cts.CancelAfter(TimeSpan.FromSeconds(10));

            try
            {
                // 直接使用await调用
                // await asyncDemo.FetchWebsiteAsync(asyncCts.Token);
                // 使用 Task.Factory.StartNew 启动异步操作
                //  await asyncDemo.FetchWebsiteWithStartNew(asyncCts.Token);

                // 使用 Task.Run 启动异步操作
                //  await asyncDemo.FetchWebsiteWithRun(asyncCts.Token);
            }
            catch (Exception ex)
            {
                // 在主线程中捕获异常
                Console.WriteLine($"在 Main 中捕获到的异常: {ex.Message}");
            }

            Console.WriteLine("操作完成。");
            #endregion
             #region 线程安全对象
            var threadConcurrent = new ThreadConcurrent();
            Console.WriteLine("*****************输出普通线程集合***********");
            threadConcurrent.ThreadList(); //结果是无序的
            Console.WriteLine("*****************输出线程安全ConcurrentBag集合***********");
            threadConcurrent.ThreadInterlockedList(); //结果是有序的
            Console.WriteLine("*****************输出普通int集合累加***********");
            threadConcurrent.ThreadInt();
            Console.WriteLine("*****************输出原子Interlocked.Increment操作累加***********");
            threadConcurrent.ThreadInterlockInt();

            #endregion
#endif
            #region 线程锁
            var threadLock = new ThreadLock();
            Console.WriteLine("*****************输出线程锁的结果**************************");
           // threadLock.ThreadLockObject();
            Console.WriteLine("*****************输出原子锁的结果**************************");
            threadLock.ThreadInterlocked();
            #endregion

            Console.ReadLine();
        }




        #region 并行编程
        #endregion

        #region 异步编程
        #endregion


    }


}
