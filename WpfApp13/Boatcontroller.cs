﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApp1
{
    public class Boatcontroller
    {

        //public void EmptyDatabase()
        //{
        //    using (Database context = new Database())
        //    {
        //        context.Database.Delete();
        //    }
        //}

        //Deze methode returnd true als naam en gewicht zijn ingevoerd (anders false)
        public Boolean WhiteCheck(string name, string weight)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(weight))
            {

                MessageBoxResult DataIncorrect = MessageBox.Show(
                    "U heeft niet alle gegevens ingevuld",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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

                MessageBoxResult WeightIncorrect = MessageBox.Show(
                    "Het gewicht moet een getal zijn",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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
                    MessageBoxResult NameIncorrect = MessageBox.Show(
                       "Deze bootnaam bestaat al",
                       "Melding",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);

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
              
                    Enum.TryParse(type, out Boat.BoatType MyType);
                    var boot1 = new Boat
                    {
                        Name = name,
                        Type = MyType,
                        NumberOfRowers = rowers,
                        Weight = weight,
                        Steering = steeringwheel


                    };

                    context.Boats.Add(boot1);


                    context.SaveChanges();
                }
      
                

            }

        

        //public List<Boat> BoatList()
        //{
        //    using (Database context = new Database())
        //    {

        //        var boats = (from s in context.Boats
        //                     orderby s.BoatID
        //                     select s).ToList<Boat>();

        //        return boats;
        //    }
        //}

        //public void Print()
        //{
        //    using (Database context = new Database())
        //    {

        //        foreach (var boat in BoatList())
        //        {

        //            Console.WriteLine($"ID: {boat.BoatID}, Name: {boat.Name}, BoatType : {boat.Type}, Roeiers: {boat.NumberOfRowers}, gewicht: {boat.Weight}, Stuur {boat.Steering}");
        //        }
        //        Console.ReadKey();
        //    }
        //}
      

    }
}
