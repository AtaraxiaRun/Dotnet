namespace BuilderPattern
{
    /// <summary>
    /// 构造者模式
    /// 总结：构造器模式可以用来构建复杂的对象，将每一个部分的构建都交给一个单独的方法控制，然后通过多个链式的构建，构造出最终的结果
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            //建立导航器
            Cook cook = new Cook();
            PizzaBuilder hawaiianPizzaBuilder = new HawaiianPizzaBuilder();
            PizzaBuilder spicyPizzaBuilder = new SpicyPizzaBuilder();

            cook.SetPizzaBuilder(hawaiianPizzaBuilder);
            cook.ConstructPizza();

            Pizza hawaiian = cook.GetPizza();
            Console.WriteLine(hawaiian.ToString());

            cook.SetPizzaBuilder(spicyPizzaBuilder);
            cook.ConstructPizza();

            Pizza spicy = cook.GetPizza();
            Console.WriteLine(spicy.ToString());
            Console.WriteLine("**********以下为自建汽车类**********");
            var carBuilder = new CarBuilder();
            var car = carBuilder.CreateNewCar();
            carBuilder.BuilderWheels();
            carBuilder.BuilderEngine();
            Console.WriteLine(car.ToString());

        }
    }
}
