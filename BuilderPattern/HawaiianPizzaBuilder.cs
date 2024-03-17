using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    /// <summary>
    /// 具体构造者类：夏威夷披萨
    /// </summary>
    public class HawaiianPizzaBuilder : PizzaBuilder
    {
        public override void BuildCheese()
        {
            pizza.Cheese = "夏威夷奶酪";
        }

        public override void BuildDough()
        {
            pizza.Dough = "夏威夷面团";
        }

        public override void BuildSauce()
        {
            pizza.Sauce = "夏威夷酱汁";
        }

        public override void BuildTopping()
        {
            pizza.Topping = "夏威夷配料";
        }
    }
}
