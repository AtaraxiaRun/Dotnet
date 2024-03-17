using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    /// <summary>
    /// 构造者抽象类
    /// </summary>
    public abstract class PizzaBuilder
    {
        protected Pizza pizza;

        public void CreateNewPizzaProduct()
        {
            pizza = new Pizza();
        }

        public Pizza GetPizza() { return pizza; }

        public abstract void BuildDough();
        public abstract void BuildSauce();
        public abstract void BuildCheese();
        public abstract void BuildTopping();
    }
}
