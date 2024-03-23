using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleInterface
{
    public class MyClass2 : MyAbstractClass
    {
        public override void Close()
        {
            Console.WriteLine("Close");
        }

      
    }
}
