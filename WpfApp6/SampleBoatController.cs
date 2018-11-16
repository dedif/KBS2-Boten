

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
                // public Boat(int id, string name, string type, int weight, int amountOfRowers, bool steerman)
                new Boat(1,"CRX", "Scull", 30, 1, false),
                new Boat(2,"Scirocco", "Board", 30, 1, false),
                new Boat(3, "MR2", "Skiff", 60, 4, true)
            };
        }
    }
}