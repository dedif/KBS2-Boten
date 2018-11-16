

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
                new Boat(1,"CRX", "Scull", 30, 1, false),
                new Boat(2, "MR2", "Skiff", 60, 4, true)
            };
        }
    }
}