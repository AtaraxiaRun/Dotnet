using System.Collections.Concurrent;

namespace NetCore.ConsoleThread
{
    internal class Program
    {
        /// <summary>
        /// 注意：异步的async 需要返回值是Task
        /// </summary>
        /// <returns></returns>
        static async Task Main()
        {
            #region 并发编程
#if false
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
#endif
            #endregion
            #region 并发编程-Delay,ConfigureAwait
#if true
            //await Task.Delay(2000).ConfigureAwait(false);
            //Console.WriteLine("WriteAsync1_2");
            Console.WriteLine("1");
            ConcurrencyDemo demo = new ConcurrencyDemo(3);
            var task1 = demo.WriteAsync1(); //1.拿到task1就代表结果已经出来了，实际的开始执行start并不是Task.WaitAll触发的,调用demo.WriteAsync1()的时候方法就开始触发了 2.调用 var task1 = demo.WriteAsync1(); 通过var task1并不会阻塞主线程,后面“111”会马上输出，如果是用  await demo.WriteAsync1(); 就会阻塞主线程，“111”就会等待输出
            Console.WriteLine("111");
            var task2 = demo.WriteAsync2();  //看看  await Task.Delay(2000); 问： 是不是会等2秒输出 答： 是的会等待两秒输出
            Task.WaitAll(task1, task2);  //Task.WaitAll是没有启动功能的，Task.WaitAll只是会在任何情况下都等待所有线程执行完毕
            Console.WriteLine("4");

            /*
             我认为的输出： 1，2，3，4   或者 1，3，2，4 因为Task.WaitAll并发执行是无序的
             实际的输出：   1，2，3，4   或者 1，3，2，4 牛逼
             */
            Console.ReadLine();
#endif
            #endregion
            #region 并行编程
#if false
             
            ParallelismDemo parallelismDemo = new ParallelismDemo(3);

            // 创建 CancellationTokenSource 以便在需要时取消操作
            CancellationTokenSource cts = new CancellationTokenSource();
            //cts.Cancel(); //如果需要取消,则取消注释

            parallelismDemo.ProcessDataInParallel(cts.Token);
#endif
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
#if false
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
#endif
            #endregion
            #region 线程安全对象
#if false
            var threadConcurrent = new ThreadConcurrent();
            Console.WriteLine("*****************输出普通线程集合***********");
            threadConcurrent.ThreadList(); //结果是无序的
            Console.WriteLine("*****************输出线程安全ConcurrentBag集合***********");
            threadConcurrent.ThreadInterlockedList(); //结果是有序的
            Console.WriteLine("*****************输出普通int集合累加***********");
            threadConcurrent.ThreadInt();
            Console.WriteLine("*****************输出原子Interlocked.Increment操作累加***********");
            threadConcurrent.ThreadInterlockInt();
#endif
            #endregion
            #region 线程锁
#if false
            var threadLock = new ThreadLock();
            Console.WriteLine("*****************输出线程锁的结果**************************");
           // threadLock.ThreadLockObject();
            Console.WriteLine("*****************输出原子锁的结果**************************");
            threadLock.ThreadInterlocked();
#endif
            #endregion
            Console.ReadLine();
        }


    }


}
