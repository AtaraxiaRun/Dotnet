using System.Linq;
using static NetCore.ConsoleDelegate.Event.Program;

namespace NetCore.ConsoleDelegate.Event
{
    /// <summary>
    /// 委托和事件的区别
    /// 委托：
    /// 使用场景：
    /// 1.当方法内部有短小的方法只执行一次，或者只在方法内部调用的时候可以声明委托实现
    /// 2.当作方法的参数传递到方法内部进行执行，比如linq方法，提高方法的可用性
    /// 3.通过在设计中使用委托，这些不同的组件松散地耦合在一起。 这样可提供多种优势。 可轻松创建新的输出机制并将它们附加到系统中 （通过+-增加委托的功能）
    /// 事件：
    /// 使用场景：
    /// 1.订阅者发布者：由事件发布信息，触发绑定的发布者
    /// 我认为事件是一个行为，行为触发后可以产生回调的方法，委托就是执行一个函数
    /// </summary>
    /// 

    //【没有搞懂事件与委托的本质区别是什么，到底微软用来解决什么问题】
    internal class Program
    {
        public delegate int Comparion<in T>(T left, T right); //声明委托类型
        Comparion<int> actionComparion;
         static void Main(string[] args)
        {
            #region 委托
            //1.默认调用的就是Func<T1,T2,out Tresult>委托
            var add = (int a, int b) =>
            {
                return a + b;
            };
            Console.WriteLine(add(1, 2));

            //2.Linq模式的委托
            var numbers = new List<int>() { 1, 2, 3, 11 };
            var smallNumbers = numbers.Where(u => u < 10);
            //3.委托链条：Logger.WriteMessage -= LoggingMethods.LogToConsole; Logger.WriteMessage += LoggingMethods.LogToConsole;
            #endregion

            #region 事件
            //EventArgs 所有事件类的基类
            #endregion

        }

        /// <summary>
        /// Linq就是用的委托，int 是linq中的u,bool 是linq方法的返回值，可以是多个表达式
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        static IEnumerable<int> Where(Func<int, bool> predicate)
        {
            var numbers = new List<int>() { 1, 2, 3, 11 };
            return numbers.Where(predicate);
        }
    }
}
