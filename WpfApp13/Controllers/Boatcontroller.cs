﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp13;

namespace Controllers
{
    public class Boatcontroller
    {
        private string notification;
        //public void EmptyDatabase()
        //{
        //    using (Database context = new Database())
        //    {
        //        context.Database.Delete();
        //    }
        //}

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
            using (Database context = new Database())
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
            using (Database context = new Database())
            {
              
                    Enum.TryParse(type, out Boat.BoatType MyType);
                Boat boot1 = new Boat(name, MyType, rowers, weight, steeringwheel);
               
                    context.Boats.Add(boot1);


                    context.SaveChanges();
                }
      
                

            }



        public List<Boat> BoatList()
        {
            using (Database context = new Database())
            {

                var boats = (from s in context.Boats
                             orderby s.BoatID
                             select s).ToList<Boat>();

                return boats;
            }
        }


    }
}