using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Controllers
{
    public class BoatController
    {
        private string notification;

        public string Notification()
        {
            return notification;
        }
        //Deze methode returnt true als naam en gewicht zijn ingevoerd (anders false)
        
        public Boolean WhiteCheck(string name, string weight)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(weight))
            {
                notification = "U heeft niet alle gegevens ingevuld";
                
                return false;

            }
            else
            {
                return true;
            }
        }

        //Deze methode returnd true als gewicht is ingeverd als cijfers (anders false)
        public Boolean WeightCheck(string weight)
        {
     
            try
            {
                double Weight = double.Parse(weight);
                return true;
            }
            catch
            {

                notification = "Het gewicht moet een getal zijn";

                return false;
            }
        }

        //Deze methode returnd true als naam niet voor komt (anders false)
        public Boolean NameCheck(string name)
        {

            using (DataBase context = new DataBase())

            {
                var CountNames = (from b in context.Boats
                                  where b.Name == name
                                  select b).ToList<Boat>();
                if (CountNames.Count > 0)
                {
                    notification = "Deze bootnaam bestaat al";
                   

                    return false;

                }
                else
                {
                    return true;
                }
            }
        }
//Deze methode voegt een boot toe aan de database
        public void AddBoat(string name, string type, int rowers, double weight, bool steeringwheel)
        {
            notification = "";

            using (DataBase context = new DataBase())

            {
              
                    Enum.TryParse(type, out Boat.BoatType MyType);
                DateTime CreatedAt = DateTime.Now;
                Boat boot1 = new Boat(name, MyType, rowers, weight, steeringwheel, CreatedAt);
               
                    context.Boats.Add(boot1);


                    context.SaveChanges();
                }
      
                

            }
        public void AddDiploma(List<CheckBox> list)
        {

        //Deze methode update en boot als verwijdert in de database
        public void DeleteBoat(int boatID)
        {

            using (DataBase context = new DataBase())

            {
                Boat delBoat = context.Boats.Where(d => d.BoatID == boatID).First();

                if (delBoat != null)
                {

                    delBoat.DeletedAt = DateTime.Now;

                    context.SaveChanges();
                }

                context.Boats.Add(delBoat);


                context.SaveChanges();
            }

        }

            using (DataBase context = new DataBase())
            {
                //De BoatID van de laatst toegvoegde boat
                var BoatID = (from data in context.Boats
                              orderby data.BoatID descending
                              select data.BoatID).First();
                //Elke checkbox voor diploma's worden toegeoegt aan een list
                foreach (CheckBox box in list)
                {
                    //Als de checkbox is aangevinkt dat wordt dit toegevoegd aan de database
                    if (box.IsChecked == true)
                    {
                        int diplomaID = int.Parse(box.Tag.ToString());
                        Boat_Diploma Diploma = new Boat_Diploma(BoatID, diplomaID);
                        context.Boat_Diplomas.Add(Diploma);
                        context.SaveChanges();
                    }
                }
            }
        }


        public List<Boat> BoatList()
        {

            using (DataBase context = new DataBase())
            {

                var boats = (from s in context.Boats
                             orderby s.BoatID
                             select s).ToList<Boat>();

                return boats;
            }
        }
		
		public Boat GetBoatWithName(string name)
        {
            using (var context = new DataBase())
            {
                return (from boat in context.Boats where boat.Name.Equals(name) select boat).First();
            }
        }

    }
}
