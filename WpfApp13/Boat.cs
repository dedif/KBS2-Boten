using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Boat
    {
        public enum BoatType {Scull, Skiff, Board}

        public int BoatID { get; set; }
        public string Name { get; set; }
        public BoatType Type { get; set; }
        public int AmountOfRowers { get; set;}
        public double Weight { get; set; }
        public bool Steering { get; set; }
        public string Status { get; set; }

        public Boat(string name, BoatType type, int amountOfRowers, double weight, bool steering)
        {
            Name = name;
            Type = type;
            Weight = weight;
            AmountOfRowers = amountOfRowers;
            Steering = steering;
        }


    }
}
