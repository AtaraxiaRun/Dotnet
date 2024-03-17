using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    /// <summary>
    /// 产品类
    /// </summary>
    public class Pizza
    {
        /// <summary>
        /// 面团
        /// </summary>
        public string Dough { get; set; }
        /// <summary>
        /// 酱汁
        /// </summary>
        public string Sauce { get; set; }

        /// <summary>
        /// 奶酪
        /// </summary>
        public string Cheese { get; set; }
        /// <summary>
        /// 配料
        /// </summary>
        public string Topping { get; set; }

        /// <summary>
        /// 所有类都可以重写ToString()类
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Dough:{Dough} ,Sauce:{Sauce},Cheese:{Cheese},Topping:{Topping}";
        }
    }
}
