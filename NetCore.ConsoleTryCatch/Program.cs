namespace NetCore.ConsoleTryCatch
{
    /// <summary>
    /// 总结：几乎所有情况下Finally的值都会执行，即使是catch 中return，还是try正常运行中return都会执行finally中的内容后退出
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TryCatchFinallyException());
            Console.WriteLine(TryCatchFinally());

        }

        /// <summary>
        /// 带异常的TryCatch
        /// 猜测：输出结果是什么？  
        /// 返回：
        /// Finally-0
        /// 0 
        /// </summary>
        public static int TryCatchFinallyException()
        {
            int i = 0;
            int k = 0;
            try
            {
                i = 10 / k;
                return i;
            }
            catch
            {
                return i;
            }
            finally
            {
                Console.WriteLine($"Finally-{i}");
            }
        }


        /// <summary>
        /// 不带异常的TryCatch 
        /// 猜测：输出结果是什么？  
        /// Finally-10
        /// 10 
        /// </summary>
        public static int TryCatchFinally()
        {
            int i = 0;
            int k = 1;
            try
            {
                i = 10 / k;
                return i;
            }
            catch
            {
                return i;
            }
            finally
            {
                Console.WriteLine($"Finally-{i}");
            }
        }
    }
}
