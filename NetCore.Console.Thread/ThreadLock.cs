using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    /// <summary>
    /// 线程锁:1.Lock锁 2.Interlocked.Exchange 原子锁   
    /// Interlocked原子操作类可以对单个变量(整型（一般都是用于整型），引用类型，集合)   做更新的原子操作，不需要使用Lock
    /// </summary>
    public class ThreadLock
    {



        #region 锁

        /// <summary>
        /// 线程锁
        /// </summary>
        public void ThreadLockObject()
        {
            object lockObj = new object();
            int i = 0;
            var tasks = Enumerable.Range(0, 50000000).Select(u => Task.Factory.StartNew(() =>
            {
                lock (lockObj)
                {
                    i++;
                }
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"输出i的值：{i}"); //输出i的值：50000000
        }

        /// <summary>
        /// 原子锁：每次只有一个线程可以进来，不会阻塞，线程判断等于0，那么可以进，第二个线程进来不满足条件直接走，不会像
        /// lock关键字那样进行等待
        /// </summary>
        public void ThreadInterlocked()
        {
            int i = 0;
            var tasks = Enumerable.Range(0, 5).Select(u => Task.Factory.StartNew(() =>
            {
                if (Interlocked.Exchange(ref i, 1) == 0) //判断是否等于0，等于0赋值为1
                {
                    Console.WriteLine($"ManagedThreadId_{Thread.CurrentThread.ManagedThreadId}_原子锁进来了_i_{i}");
                    Interlocked.Exchange(ref i, 0); //执行结束赋值为0
                }
            }));
            Task.WaitAll(tasks.ToArray());
            //输出：ManagedThreadId_11_原子锁进来了_i_1   ，就一条，非常正常
        }
        #endregion
    }

}
