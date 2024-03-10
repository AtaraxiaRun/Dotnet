using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    /// <summary>
    /// 线程安全的对象
    /// </summary>
    public class ThreadConcurrent
    {
        #region 线程安全的对象
        /*
        ConcurrentBag<T>：无序集合（List<T>是有序集合），用于存储对象，可以进行线程安全的添加和移除操作，适合于那些元素的插入和移除次数多于迭代次数，并且不需要排序或索引功能的并发应用场景

        ConcurrentDictionary<TKey, TValue>：表示键值对的线程安全集合（可以看做单线程版本的Dictionary<TKey, TValue>），可以用于存储键和值，并支持线程安全的添加、移除和更新操作。

         */
        /// <summary>
        /// 普通线程集合
        /// </summary>
        public void ThreadList()
        {
            string name = "王锐";
            List<string> listNames = new List<string>();
            var taskList = Enumerable.Range(0, 5).Select(u => Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    listNames.Add($"ManagedThreadId_{Thread.CurrentThread.ManagedThreadId}_{name}_{i}");
                }
            })).ToList();
            Task.WaitAll(taskList.ToArray());
            //输出值，看看值的插入是不是有序的
            foreach (var item in listNames)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("输出完毕");

        }


        /// <summary>
        /// 原子线程集合
        /// </summary>

        public void ThreadInterlockedList()
        {
            string name = "王锐";
            var concurrentBagNames = new ConcurrentBag<string>();
            var taskList = Enumerable.Range(0, 5).Select(u => Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    concurrentBagNames.Add($"ManagedThreadId_{Thread.CurrentThread.ManagedThreadId}_{name}_{i}");
                }
            })).ToList();

            Task.WaitAll(taskList.ToArray());
            //输出值，看看值的插入是不是有序的
            foreach (var item in concurrentBagNames)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("输出完毕");
        }

        /// <summary>
        /// 直接相加
        /// </summary>
        public void ThreadInt()
        {
            int i = 0;
            //5000000(五百万)的大值是因为数量太小，体现不出差异
            var tasks = Enumerable.Range(0, 5000000).Select(_ => Task.Factory.StartNew(() =>
            {
                i++;
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"i等于{i}"); //i等于4999354
        }

        /// <summary>
        /// 原子的相加
        /// </summary>

        public void ThreadInterlockInt()
        {
            int i = 0;
            //5000000(五百万)的大值是因为数量太小，体现不出差异
            var tasks = Enumerable.Range(0, 5000000).Select(_ => Task.Factory.StartNew(() =>
            {
                Interlocked.Increment(ref i);
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"i等于{i}");//i等于5000000

        }

        #endregion

    }
}
