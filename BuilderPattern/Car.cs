using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    public class Car
    {
        
        /// <summary>
        /// 轮子
        /// </summary>
        public string Wheels { get; set; }

        /// <summary>
        /// 发动机
        /// </summary>
        public string Engine { get; set; }

        public override string ToString()
        {
            return $"Wheels：{Wheels} ,Engine：{Engine}";
        }
    }
}
