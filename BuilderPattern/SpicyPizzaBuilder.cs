using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    /// <summary>
    /// 具体的构造器：辛辣味的披萨
    /// </summary>
    public class SpicyPizzaBuilder : PizzaBuilder
    {
        public override void BuildCheese()
        {
            pizza.Cheese = "辛辣味奶酪";
        }

        public override void BuildDough()
        {
            pizza.Dough = "辛辣味面团";
        }

        public override void BuildSauce()
        {
            pizza.Sauce = "辛辣味酱汁";
        }

        public override void BuildTopping()
        {
            pizza.Topping = "辛辣味配料";
        }
    }
}
