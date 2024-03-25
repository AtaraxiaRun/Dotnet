using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{

    #region 使用ReaderWriterLockSlim来保护对共享数据并发读，与同步写访问
    /// <summary>
    /// 使用ReaderWriterLockSlim来保护对共享数据并发读，与同步写访问：
    /// </summary>
    public class ExampleReaderWriterLockSlim
    {
        private static ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private static List<int> _items = new List<int>();

        public static void Main1()
        {
            new Thread(Read).Start();
            new Thread(Read).Start();
            new Thread(Read).Start();
            new Thread(Write).Start("A");
            new Thread(Write).Start("B");
        }

        static void Read()
        {
            while (true)
            {
                _rwLock.EnterReadLock(); //增加读锁，允许多个线程同时读取数值列表，但如果有线程正在写入，它将等待直到写入完成，确保每个线程读取的结果都是一致的
                foreach (int item in _items) Thread.Sleep(10);
                _rwLock.ExitReadLock(); //释放读锁
            }
        }

        static void Write(object threadID)
        {
            while (true)
            {
                int newItem = new Random().Next(100);
                _rwLock.EnterWriteLock(); //增加写入锁，写入的场景只能有一个线程在做
                _items.Add(newItem);
                Console.WriteLine("Thread " + threadID + " added " + newItem);
                _rwLock.ExitWriteLock();  //释放写入锁
                Thread.Sleep(100);
            }
        }
    }
    #endregion
}
