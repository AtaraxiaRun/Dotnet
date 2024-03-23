using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleInterface
{
    /// <summary>
    /// 抽象类
    /// </summary>
    public abstract class MyAbstractClass
    {
        public void Show()
        {
            Console.WriteLine("MyAbstractClass_Show");
        }

        public abstract void Close();
    }
}
