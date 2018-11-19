using ConsoleApp1;
using System.Collections.Generic;
using System.Windows.Documents;

namespace WpfApp6
{
    public class SampleBoatController
    {
        public List<Boat> GetBoats()
        {
            return new List<Boat>
            {
                // public Boat(string name, BoatType type, int amountOfRowers, double weight, bool steering)
                new Boat("CRX", Boat.BoatType.Skiff, 30, 1, false),
                new Boat("Scirocco", Boat.BoatType.Scull, 30, 1, false),
                new Boat("MR2", Boat.BoatType.Scull, 60, 4, true)
            };
        }
    }
}