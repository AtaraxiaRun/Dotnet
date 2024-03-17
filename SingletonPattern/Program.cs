namespace SingletonPattern
{
    #region 单例模式思考
    /**
     * 
     * 博客原文：https://www.kancloud.cn/wizardforcel/learning-hard-csharp/111562
     * 了解完了一些关于单例模式的基本概念之后，下面就为大家剖析单例模式的实现思路的，因为在我自己学习单例模式的时候，咋一看单例模式的实现代码确实很简单，也很容易看懂，但是我还是觉得它很陌生（这个可能是看的少的，或者自己在写代码中也用的少的缘故），而且心里总会这样一个疑问——为什么前人会这样去实现单例模式的呢？他们是如何思考的呢？后面经过自己的琢磨也就慢慢理清楚单例模式的实现思路了，并且此时也不再觉得单例模式陌生了，下面就分享我的一个剖析过程的：
     * 我们从单例模式的概念（确保一个类只有一个实例,并提供一个访问它的全局访问点）入手，可以把概念进行拆分为两部分：（1）确保一个类只有一个实例；
     * （2）提供一个访问它的全局访问点；
     * 下面通过采用两人对话的方式来帮助大家更快掌握分析思路：

菜鸟：怎样确保一个类只有一个实例了？

老鸟：那就让我帮你分析下，你创建类的实例会想到用什么方式来创建的呢？

新手：用new关键字啊，只要new下就创建了该类的一个实例了，之后就可以使用该类的一些属性和实例方法了

老鸟：那你想过为什么可以使用new关键字来创建类的实例吗？

菜鸟：这个还有条件的吗？........., 哦，我想起来了，如果类定义私有的构造函数就不能在外界通过new创建实例了（注：有些初学者就会问，有时候我并没有在类中定义构造函数为什么也可以使用new来创建对象，那是因为编译器在背后做了手脚了，当编译器看到我们类中没有定义构造函数，此时编译器会帮我们生成一个公有的无参构造函数）

老鸟：不错，回答的很对，这样你的疑惑就得到解答了啊

菜鸟：那我要在哪里创建类的实例了？

老鸟：你傻啊，当然是在类里面创建了（注：这样定义私有构造函数就是上面的一个思考过程的，要创建实例，自然就要有一个变量来保存该实例把，所以就有了私有变量的声明,但是实现中是定义静态私有变量,朋友们有没有想过——这里为什么定义为静态的呢?对于这个疑问的解释为：每个线程都有自己的线程栈，定义为静态主要是为了在多线程确保类有一个实例）

菜鸟：哦，现在完全明白了，但是我还有另一个疑问——现在类实例创建在类内部，那外界如何获得该的一个实例来使用它了？

老鸟：这个，你可以定义一个公有方法或者属性来把该类的实例公开出去了（注：这样就有了公有方法的定义了，该方法就是提供方法问类的全局访问点）
     * **/
    #endregion

  
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }



    /// <summary>
    /// 简单单例模式
    /// </summary>
    public class Singleton
    {
        private static Singleton _instance;
        private Singleton() { }

        public static Singleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 多线程版本单例模式
    /// </summary>
    public class SingletonThreading
    {
        private static SingletonThreading _instance ;
        private static readonly object object_lock = new object();

        private SingletonThreading() { }

        public static SingletonThreading GetInstance()
        {
            if (_instance == null)
            {
                lock (object_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SingletonThreading();
                    }
                }
            }
            return _instance;
        }
    }

 
}
