using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    #region 并行编程
    public class ParallelismDemo
    {
        private int[] data;

        public ParallelismDemo(int dataSize)
        {
            data = new int[dataSize];
            for (int i = 0; i < dataSize; i++)
            {
                data[i] = i;
            }
        }


        /// <summary>
        /// 处理数据的并行操作
        /// </summary>
        /// <param name="ct"></param>
        public void ProcessDataInParallel(CancellationToken? ct = null)
        {
            //设置并行操作选项
            ParallelOptions po = new ParallelOptions()
            {
                CancellationToken = ct ?? CancellationToken.None,
                MaxDegreeOfParallelism = Environment.ProcessorCount

            };

            try
            {
                Console.WriteLine("************使用Parallel.For 进行并行处理************");
                // 使用Parallel.For 进行并行处理
                //优点：1.最大化利用CPU性能 2.比Parallel.For性能更好
                //缺点：2.阻塞主线程
                //背景：并行迭代，阻塞主线程，数组便利
                Parallel.For(0, data.Length, po, item =>
                {
                    if (po.CancellationToken.IsCancellationRequested)
                    {
                        po.CancellationToken.ThrowIfCancellationRequested();
                    }
                    int result = DoWork(item); //并行处理任务
                    Console.WriteLine($"元素 {item} 的处理结果是 {result}，由线程 {Thread.CurrentThread.ManagedThreadId} 处理。");

                });

                Console.WriteLine("************使用Parallel.ForEach 对数组进行并行迭代************");
                // 使用Parallel.ForEach 对数组进行并行迭代
                //优点：1.最大化利用CPU性能
                //缺点：2.阻塞主线程
                //背景：并行迭代，阻塞主线程，数组便利
                Parallel.ForEach(data, po, item =>
                {
                    int result = DoWork(item);
                    Console.WriteLine($"元素 {item} 的处理结果是 {result}，由线程 {Thread.CurrentThread.ManagedThreadId} 处理。");
                });

                Console.WriteLine("************使用Parallel.ForEach 和 Partitioner 进行数据处理************");
                // 使用Parallel.ForEach 和 Partitioner 进行数据处理
                //优点：1.最大化利用CPU性能
                //缺点：2.阻塞主线程
                //背景：1.Partitioner.Create可以创建分区，每个分区只执行一次函数，而不是每次迭代调用一次分区
                //      2.处理速度相对均匀 ，每条的数据处理时间都不固定
                //      3.数据量大,比如几千，几万条

                var rangePartitioner = Partitioner.Create(0, data.Length); //创建Partitioner以提供一个更有效的数据分区策略
                Parallel.ForEach(rangePartitioner, po, (range, loopstate) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        int result = DoWork(i);
                        Console.WriteLine($"元素 {i} 的处理结果是 {result}，由线程 {Thread.CurrentThread.ManagedThreadId} 处理。");
                    }
                });

            }
            catch (OperationCanceledException oce)
            {
                Console.WriteLine($"操作被取消，消息内容： {oce.Message}");
            }
            catch (AggregateException ae)
            {
                // 处理并行操作中的所有异常
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine($"处理的异常类型：{e.GetType()},消息内容：{e.Message}");
                }
            }

            catch (Exception ex)
            {

                Console.WriteLine($"发生异常：{ex.Message}");
            }
        }


        /// <summary>
        /// 实际进行的操作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int DoWork(int value)
        {
            //具体的业务逻辑
            return value * value; // 示例：计算平方
        }
    }
    #endregion
}
