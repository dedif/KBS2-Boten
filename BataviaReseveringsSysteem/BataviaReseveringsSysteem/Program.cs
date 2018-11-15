using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Les_14
{
    class Program
    {
        static void Main(string[] args)
        {
            Boatcontroller b = new Boatcontroller();
            b.EmptyDatabase();

                b.AddBoat("haai", "hoog", 2, 5.35, true);
            b.AddBoat("walvis", "laag", 7, 5.35, false);
            b.AddBoat("haai", "midden", 2, 2000, true);

            b.Print();
        }

     


    
    }
}
