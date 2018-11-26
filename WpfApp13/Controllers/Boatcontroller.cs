using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp13;
using System.Windows;

namespace Controllers
{
    public class Boatcontroller
    {
        private string notification;
        public string Notification()
        {
            return notification;
        }

        //Deze methode returnd true als naam en gewicht zijn ingevoerd (anders false)
        public bool WhiteCheck(string name, string weight)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(weight)) return true;
            MessageBox.Show(
                "U heeft niet alle gegevens ingevuld",
                "Melding",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }

        //Deze methode returnd true als gewicht is ingeverd als cijfers (anders false)
        public bool WeightCheck(string weight)
        {
     
            try
            {
                double.Parse(weight);
                return true;
            }
            catch
            {
                MessageBox.Show(
                    "Het gewicht moet een getal zijn",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }

        //Deze methode returnd true als naam niet voor komt (anders false)
        public bool NameCheck(string name)
        {
            using (var context = new Database())
            {
                var countNames = (from b in context.Boats
                                  where b.Name == name
                                  select b).ToList<Boat>();
                if (countNames.Count <= 0) return true;
                MessageBox.Show(
                    "Deze bootnaam bestaat al",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }
//Deze methode voegt een boot toe aan de database
        public void AddBoat(string name, string type, int rowers, double weight, bool steeringwheel)
        {
            using (var context = new Database())
            {
                Enum.TryParse(type, out Boat.BoatType MyType);
                var boot1 = new Boat(name, MyType, rowers, weight, steeringwheel);
                context.Boats.Add(boot1);
                context.SaveChanges();
            }
        }

        public List<Boat> BoatList()
        {
            using (var context = new Database())
            {

                var boats = (from s in context.Boats
                             orderby s.BoatID
                             select s).ToList();

                return boats;
            }
        }

        public Boat GetBoatWithName(string name)
        {
            using (var context = new Database())
                return (from boat in context.Boats where boat.Name.Equals(name) select boat).First();
        }
    }
}
