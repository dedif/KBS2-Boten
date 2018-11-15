using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Boat
    {
        public enum type {Scull, Skiff, Board}

        public int Id { get; set; }
        public string Name { get; set; }
        public type Type { get; set; }
        public int AmountRowers { get; set;}
        public double Weight { get; set; }
        public bool SteeringWheel { get; set; }



    }
}
