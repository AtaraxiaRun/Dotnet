namespace SimpleFactory
{
    /// <summary>
    /// 简单工厂模式
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            IAnimal cat = AnimalFactory.GetAnimalFactory("cat");
            IAnimal dog = AnimalFactory.GetAnimalFactory("dog");
            cat.SayHi();
            dog.SayHi();
        }
    }
    #region 方便扩展简单工厂
    public interface IAnimal
    {
        void SayHi();
    }

    public class AnimalFactory
    {
        public static IAnimal GetAnimalFactory(string type)
        {
            if (type == "cat")
            {
                return new Cat();
            }
            else if (type == "dog")
            {
                return new Dog();
            }
            return null;
        }
    }

    public class Cat : IAnimal
    {
        public void SayHi()
        {
            Console.WriteLine("你好 我是一只猫");
        }
    }

    public class Dog : IAnimal
    {
        public void SayHi()
        {
            Console.WriteLine("你好 我是一只狗");
        }
    }
    #endregion
    #region 数据库简单工厂:隐藏，封装的逻辑

    public class DbFactory
    {
        public static DbFactory GetDbFactory()
        {
            //隐藏，封装的逻辑,满足条件后进行返回创建，也方便进行扩展，如果创建对象的逻辑，需要发生变化，并且引用到了几十个地方，那么我们只需要更改这一处地方就可以
            return new DbFactory();

        }
    }
    #endregion
}
