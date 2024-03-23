using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    /// <summary>
    /// 使用Monitor保证同步访问对象
    /// </summary>
    public class TaskThreadMonitor
    {
        public void ThreadMonitorTime() 
        {
            object lockObject = new object();
            bool lockTaken = false;

            try
            {
                // 尝试获取锁，等待最多20秒
                Monitor.TryEnter(lockObject, TimeSpan.FromSeconds(20), ref lockTaken);

                if (lockTaken)
                {
                    // 成功获取到锁
                    // 在这里处理线程安全的代码
                }
                else
                {
                    // 在20秒内没有获取到锁
                    // 在这里处理未能获取锁的情况，比如记录日志、尝试其他操作等
                }
            }
            finally
            {
                // 如果获取了锁，最后确保释放
                if (lockTaken)
                {
                    Monitor.Exit(lockObject);
                }
            }

        }
    }
}
