using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleGc.DisposeText
{
    /// <summary>
    /// 如果你的类里面封装了非托管资源，那么你需要手动实现IDisposable接口释放这个对象
    /// </summary>
    public class FileWriter : IDisposable
    {
        private bool disposed = false;  //资源是否已经释放
        private StreamWriter streamWriter; //非托管资源，因为对象虽然是托管资源，但是对象操作的文件是非托管资源

        public FileWriter(string filePath)
        {
            streamWriter = new StreamWriter(filePath, append: true);
        }

        public void WriteToFile(string text)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            streamWriter.WriteLine(text);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //通知垃圾回收器不需要再调用对象的终结器（析构函数）。
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放非托管资源：true 释放托管资源与非托管资源 false只释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {

            if (!disposed)
            {
                Console.WriteLine("FileWriter销毁前...");
                if (disposing)
                {
                    //释放托管资源
                    if (streamWriter != null)
                    {
                        streamWriter.Close();  //虽然Close里面会触发Dispose()方法，为了代码的清晰可读性，下面再次调用Dispose()也是可以的，因为Dispose()方法是幂等性的
                        streamWriter.Dispose();
                        streamWriter = null; //显式地将对象引用设置为null，这样做的目的是为了从逻辑上释放对该对象的引用
                    }
                }
                //释放非托管资源
                disposed = true;
            }
            Console.WriteLine("FileWriter已经销毁...");


        }
        /// <summary>
        /// 析构函数: 析构函数的语法以 ~ 符号开头，后跟类名。它不能有任何参数，也不能有返回值。
        /// GC的回收会调用对象的析构函数
        /// </summary>
        ~FileWriter()
        {
            Console.WriteLine("析构函数触发");
            Dispose(false);
        }
    }
}
