using Microsoft.Data.SqlClient;

namespace NetCore.ConsoleGc.DisposeText
{
    /// <summary>
    /// 总结：
    /// 1.托管资源：List<int>,在托管堆上分配的内存（例如，使用new关键字创建的对象实例）。由.NET环境完全管理的资源，对象不会依赖外部的资源
    /// 2.非托管资源：文件句柄（FileStream ）、网络连接（HttpClient ）、数据库连接（SqlConnection ）、图形界面句柄，不是.NET垃圾回收器的管理范围,因为网络链接与文件操作，数据库操作，打开了外部的资源，比如网络链接请求的是服务器上面的资源，数据库操作打开的是数据库的资源，文件请求打开的是文件的资源这个是GC无法去控制的，需要程序员显式管理。通过在析构函数中调用 Dispose(false)，即使忘记显式调用 Dispose 方法，非托管资源也能得到释放
    /// 3.什么时候需要手动实现IDisposable接口呢？
    /// 类里面存在非托管资源的时候需要手动释放非托管资源，比如HTTP链接，调用文件，链接数据库等
    /// 4.为什么需要析构函数呢？不是可以手动调用Dispose方法吗？
    /// 一般情况下推荐手动调用Dispose方法清理非托管资源，析构函数是为了防止对象并没有被使用，但是又没有手动调用Dispose方法，这个时候GC回收这个对象的时候就会自动调用对象的析构函数，清理对象的非托管资源
    /// 5.使用Using的话，对象离开了Using的作用域就会自动释放对象（自动调用对象的Dispose方法），如果要用Using就需要手动实现IDisposable接口
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            //1.手动调用Dispose释放
            FileWriter student = new FileWriter("example.txt");
            student.Dispose();
            //2.通过Using自动释放,离开了using的作用域就会自动释放（自动调用对象的Dispose方法），如果要用Using就需要手动实现IDisposable接口
            using (FileWriter student2 = new FileWriter("example.txt"))
            {

            }

 
        }


    }



}
