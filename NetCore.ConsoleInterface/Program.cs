namespace NetCore.ConsoleInterface
{
    /*
     接口：

        1.一组抽象的行为,也可以有具体的实现在c#8中，当没有实现接口的方法的时候，那么默认就是用接口的实现

        2.接口可以继承多个，代表同一类型的抽象的行为

        3.无法创造接口的实例

        抽象类：

        1.可以是抽象的行为，也可以是具体的实现，没有重写抽象类实现方法的时候，那么默认就是用抽象类的实现

        2. 父类只可以继承一个，代表的是一种关系，通常是一种密切相关的类的关系
        3. 无法创建抽象类的实例
     */
    internal class Program
    {
        static void Main(string[] args)
        {
            MyInterface my = new MyClass();
            my.GetShow(); //调用接口的默认实现
            new MyClass2().Show(); //调用抽象类的实现
            Console.WriteLine("Hello, World!");
        }
    }
}
