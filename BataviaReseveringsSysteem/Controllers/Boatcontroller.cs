using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Controls;
using Views;

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

        public Boolean WhiteCheck(string name, string weight, string boatLocation)
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

        public Boolean LocationCheckIfInt(string boatLocation)
        {

            try
            {
                int BoatLocation = int.Parse(boatLocation);
                return true;
            }
            catch
            {

                notification = "De locatie moet een getal zijn";

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
                                  where b.DeletedAt == null
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

        public Boolean BoatLocationCheck(int boatLocation)
        {

            using (DataBase context = new DataBase())

            {
                var CountBoatLocation = (from b in context.Boats
                                  where b.BoatLocation ==boatLocation
                                  where b.DeletedAt == null
                                  select b).ToList<Boat>();

               

                if (CountBoatLocation.Count > 0 )
                {
                    notification = "Deze boot locatie bestaat al";


                    return false;

                }
                else
                {
                    return true;
                }
            }
        }

        public Boolean EditBoatLocationCheck(int boatLocation, int boatID)
        {

            using (DataBase context = new DataBase())

            {
                var CountBoatLocation = (from b in context.Boats
                                         where b.BoatLocation == boatLocation
                                         where b.DeletedAt == null
                                         select b.BoatLocation).ToList();

                bool boatLocationCheck = context.Boats.Where(y => y.BoatID != boatID).Any(x => x.BoatLocation == boatLocation && x.DeletedAt == null);

                if (!boatLocationCheck)
                {

                    return true;

                }
                else
                {
                    notification = "Deze boot locatie bestaat al";
                    return false;
                }
            }

        }

        public Boolean BootExist(int boatID)
        {

            using (DataBase context = new DataBase())

            {
                var BoatExists = (from b in context.Boats
                                  where b.BoatID == boatID
                                  select b).ToList<Boat>();
                if (BoatExists.Count > 0)
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


        public Boolean BootParametersAreGiven(int boatID, string name, string type, int rowers, double weight, bool steering)
        {

            using (DataBase context = new DataBase())

            {
                var BoatExists = (from b in context.Boats
                                  where b.BoatID == boatID && b.Name == name && b.Type.ToString() == type && b.NumberOfRowers == rowers && b.Weight == weight && b.Steering == steering
                                  select b).ToList<Boat>();
                if (BoatExists.Count > 0)
                {
                    notification = "Parameters zijn gegeven";


                    return false;

                }
                else
                {
                    return true;
                }
            }
        }

        //Deze methode voegt een boot toe aan de database
        public void AddBoat(string name, string type, int rowers, double weight, bool steeringwheel, int boatLocation)
        {
            notification = "";

            using (DataBase context = new DataBase())

            {


                Enum.TryParse(type, out Boat.BoatType MyType);
                DateTime CreatedAt = DateTime.Now;
                Boat boot1 = new Boat(name, MyType, rowers, weight, steeringwheel, boatLocation, CreatedAt);

                context.Boats.Add(boot1);


                context.SaveChanges();
            }



        }

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
            }


        }


        public void UpdateBoat(int boatID, string name, string type, int rowers, double weight, bool steeringwheel, int boatLocation)
        {
            using (DataBase context = new DataBase())
            {
                Boat UpdateBoat = context.Boats.Where(d => d.BoatID == boatID).First();
                Enum.TryParse(type, out Boat.BoatType MyType);
                if (UpdateBoat != null)
                {
                    UpdateBoat.Name = name;
                    UpdateBoat.Type = MyType;
                    UpdateBoat.Weight = weight;
                    UpdateBoat.Steering = steeringwheel;
                    UpdateBoat.NumberOfRowers = rowers;
                    UpdateBoat.UpdatedAt = DateTime.Now;
                    UpdateBoat.BoatLocation = boatLocation;
                    try
                    {


                        context.SaveChanges();


                    }
                    catch (SqlException ex) when (ex.InnerException is SqlException sqlException && (sqlException.Number == 2627 || sqlException.Number == 2601))
                    {

                    }
                    
                    
                }
            }

        }

        public void UpdateBoatDamage(int boatID, string description, string status)
        {
            using (DataBase context = new DataBase())
            {
                Damage UpdateDamageBoat = context.Damages.Where(d => d.BoatID == boatID).First();

                if (UpdateDamageBoat != null)
                {
                    UpdateDamageBoat.Description = description;
                    UpdateDamageBoat.Status = status;
                    if (UpdateDamageBoat.Status == "Geen schade")
                    {
                        UpdateDamageBoat.TimeOfFix = DateTime.Now;
                    }
                    else
                    {
                        UpdateDamageBoat.TimeOfFix = null;
                    }
                    context.SaveChanges();
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

        public void AddDiploma(List<CheckBox> list)
        {


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


        public void Add_BoatDiploma(int diplomaID, int boatID)
        {


            using (DataBase context = new DataBase())
            {

                var BoatDiploma = new Models.Boat_Diploma
                {
                    DiplomaID = diplomaID,
                    BoatID = boatID,


                };

                context.Boat_Diplomas.Add(BoatDiploma);
                context.SaveChanges();


            }
        }

        public void Delete_BoatDiploma(int boatID, int diplomaID)
        {
            using (DataBase context = new DataBase())
            {

                var delBoatDiploma = (from x in context.Boat_Diplomas
                                      where x.BoatID == boatID && x.DiplomaID == diplomaID
                                      select x).ToList();


                context.Boat_Diplomas.RemoveRange(delBoatDiploma);

                context.SaveChanges();

            }

        }


        public List<Boat> GetBoatsReservableWithThisUsersDiplomasThatAreNotBroken()
        {
            using (var context = new DataBase())
            {
                return
                    (from boat in context.Boats
                     join boatDiploma in context.Boat_Diplomas on boat.BoatID equals boatDiploma.BoatID
                     join userDiploma in context.User_Diplomas on boatDiploma.DiplomaID equals userDiploma.DiplomaID
                     where userDiploma.UserID == LoginView.UserId
                     where boatDiploma.BoatID == boat.BoatID
                     where boat.Broken == false
                     select boat).Distinct().Concat(
                        from boat in context.Boats
                        where !(
                            from boatDiploma in context.Boat_Diplomas
                            where boatDiploma.BoatID == boat.BoatID
                            select boatDiploma).Any()
                        where boat.Broken == false
                        select boat).ToList();
            }
        }
    }
}
