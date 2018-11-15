using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Boatcontroller
    {

        public void EmptyDatabase()
        {
            using (Database context = new Database())
            {
                context.Database.Delete();
            }
        }
  
        public Boolean NameCheck(string name)
        {
            using (Database context = new Database())
            {
                var CountNames = (from b in context.Boats
                                  where b.Name == name
                                  select b).ToList<Boat>();
                if (CountNames.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public void AddBoat(string name, string type, int rowers, double weight, bool steeringwheel)
        {
            using (Database context = new Database())
            {
                if (NameCheck(name) == true)
                {
                    var boot1 = new Boat
                    {
                        Name = name,
                        Type = type,
                        AmountRowers = rowers,
                        Weight = weight,
                        SteeringWheel = steeringwheel


                    };

                    context.Boats.Add(boot1);


                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Deze boot bestaat al");
                }
                

            }

        }

        public List<Boat> BoatList()
        {
            using (Database context = new Database())
            {

                var boats = (from s in context.Boats
                             orderby s.Id
                             select s).ToList<Boat>();

                return boats;
            }
        }

        public void Print()
        {
            using (Database context = new Database())
            {

                foreach (var boat in BoatList())
                {

                    Console.WriteLine($"ID: {boat.Id}, Name: {boat.Name}, type : {boat.Type}, Roeiers: {boat.AmountRowers}, gewicht: {boat.Weight}, Stuur {boat.SteeringWheel}");
                }
                Console.ReadKey();
            }
        }
      

    }
}
