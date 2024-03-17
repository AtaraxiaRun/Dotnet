using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    public class CarBuilder
    {
        private Car car;



        public Car CreateNewCar()
        {
            car = new Car();
            return car;
        }

        public void BuilderEngine()
        {
            car.Engine = "牛逼的发动机";
        }

        public void BuilderWheels()
        {
            car.Wheels = "666的轮子";

        }

        public Car GetCar()
        {
            return car;
        }

    }
}
