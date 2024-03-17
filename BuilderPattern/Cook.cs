using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    /// <summary>
    /// 导向器
    /// </summary>
    public class Cook
    {
        private PizzaBuilder pizzaBuilder;

        public void SetPizzaBuilder(PizzaBuilder pb)
        {
            pizzaBuilder = pb;
        }

        public Pizza GetPizza()
        {
            return pizzaBuilder.GetPizza();
        }

        /// <summary>
        /// 构造披萨
        /// </summary>
        public void ConstructPizza()
        {
            pizzaBuilder.CreateNewPizzaProduct();
            pizzaBuilder.BuildDough();
            pizzaBuilder.BuildSauce();
            pizzaBuilder.BuildCheese();
            pizzaBuilder.BuildTopping();
        }
    }
}
