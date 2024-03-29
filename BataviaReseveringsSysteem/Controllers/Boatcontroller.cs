﻿using BataviaReseveringsSysteem.Controllers;
using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Views;

namespace Controllers
{
    public class BoatController
    {
        private string notification;

        public static void DeletedNotification(DateTime lastLogged)
        {
            using (DataBase context = new DataBase())
            {
                //pakt alle reserveringen waarvan de boten verwijderd zijn
                var DeletedBoats = (from data in context.Boats
                                    join a in context.Reservations
                                    on data.BoatID equals a.BoatID
                                    where data.DeletedAt > lastLogged
                                    where data.Deleted == true
                                    where a.Deleted == null
                                    select a).ToList();

                var User = (from data in context.Users
                            where data.UserID == LoginView.UserId
                            select data).Single();

                if (DeletedBoats.Count() == 1)
                {
                    MessageBoxResult DeletedNotification = MessageBox.Show(
                        "Uw afschrijvingen zijn gewijzigd omdat een boot is verwijderd",
                        "Melding",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                if (DeletedBoats.Count() > 1)
                {
                    MessageBoxResult DeletedNotification = MessageBox.Show(
                        "Uw afschrijvingen zijn gewijzigd omdat meerdere boten zijn verwijderd",
                        "Melding",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                User.LastLoggedIn = DateTime.Now;
                context.SaveChanges();
            }
        }
        public string Notification()
        {
            return notification;
        }
        //Deze methode returnt true als naam en gewicht zijn ingevoerd (anders false)

        public Boolean WhiteCheck(string name, string weight, string boatLocation)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(weight) || string.IsNullOrWhiteSpace(boatLocation))
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
// check of de locatie een int is
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
        // controlleer als de locatie al bestaat
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
        // controleer of de bootlocatie al bestaat
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
        // controlleer of de bootnaam al bestaat.
        public Boolean EditBoatNameCheck(string name, int boatID)
        {

            using (DataBase context = new DataBase())

            {
                var CountBoatLocation = (from b in context.Boats
                                         where b.Name == name
                                         where b.DeletedAt == null
                                         select b.BoatLocation).ToList();

                bool boatLocationCheck = context.Boats.Where(y => y.BoatID != boatID).Any(x => x.Name == name && x.DeletedAt == null);

                if (!boatLocationCheck)
                {

                    return true;

                }
                else
                {
                    notification = "Deze bootnaam bestaat al";
                    return false;
                }
            }

        }
      


        //Deze methode voegt een boot toe aan de database
        public void AddBoat(string name, string type, int rowers, double weight, bool steeringwheel, int boatLocation, DateTime availableAt)
        {
            notification = "";

            using (DataBase context = new DataBase())

            {


                Enum.TryParse(type, out Boat.BoatType MyType);
                DateTime CreatedAt = DateTime.Now;
                Boat boot1 = new Boat(name, MyType, rowers, weight, steeringwheel, boatLocation, CreatedAt, availableAt);

                context.Boats.Add(boot1);


                context.SaveChanges();
            }



        }

        ////Deze methode vult de gegevens van de verwijderde reservaties van de boot in
        public string ReservationContent(Reservation r)
        {
            using (DataBase context = new DataBase())
            {
                var Boat = (from data in context.Boats
                            where data.BoatID == r.BoatID
                            select data).Single();

                var Duration = r.End - r.Start;

                string content;
                content = "Naam : " + Boat.Name;
                content += "\nBegintijd: " + r.Start.Hour + ":" + r.Start.Minute;
                content += "\nDuur: " + Duration.Hours + ":" + Duration.Minutes;
                content += "\nDatum: " + r.Start.Day + "/" + r.Start.Month + "/" + r.Start.Year;
                content += "\nLocatie: " + Boat.BoatLocation;


                return content;
            }
        }
        //Deze methode stuurt een mail naar de user, die een boot had gereserveerd.
        public void SendingMail(int boatID)
        {
            using (DataBase context = new DataBase())
            {

                var ReservationList = (from data in context.Reservations
                        where data.BoatID == boatID
                        select data).ToList();

                //Voor elke reservering die is verwijderd
                foreach (Reservation r in ReservationList)
                {
                    //De user van de reservering
                    var user = (
                    from u in context.Users
                    where u.UserID == r.UserId
                    select u).Single();

                    //Dit kijkt of er al een mail is gestuurd.
                    var AlreadySendenMails = (from m in context.Mails
                                              where m.ReservationId == r.ReservationID
                                              select m).ToList();


                    if (AlreadySendenMails.Count == 0)
                    {
                        //De message van de mail
                        string sendMessage = $"Beste {user.Firstname},{Environment.NewLine}{Environment.NewLine}De boot is niet meer beschikbaar.{Environment.NewLine}De onderstaande reservering is hierdoor gewijzigd:{Environment.NewLine}{ReservationContent(r)}{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De vereniging";
                        //De titel
                        string title = "Uw reservering is gewijzigd.";
                        EmailController sendMail = new EmailController($"{user.Email}", title, sendMessage);
                        //De mail wordt verstuurd naar de gebruiker
                        Mail Mail = new Mail(r.ReservationID, title, sendMessage);
                        context.Mails.Add(Mail);
                        context.SaveChanges();
                    }
                }
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
                    var User = (from data in context.Users
                                where data.UserID == LoginView.UserId
                                select data).Single();

                    var Reservations = (from data in context.Reservations
                                        where data.BoatID == delBoat.BoatID
                                        select data).ToList();

                  
                    delBoat.DeletedAt = DateTime.Now;
                    delBoat.Deleted = true;
                    context.SaveChanges();
                    BoatController.DeletedNotification(User.LastLoggedIn);
                    User.LastLoggedIn = DateTime.Now;

                    foreach (Reservation r in Reservations)
                    {
                        r.Deleted = DateTime.Now;
                    }

                    context.SaveChanges();
                }
            }


        }

        // bewerk de boot
        public void UpdateBoat(int boatID, string name, string type, int rowers, double weight, bool steeringwheel, int boatLocation, DateTime availableAt)
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
                    UpdateBoat.AvailableAt = availableAt;
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
        // bewerk de bootschade
        public void UpdateBoatDamage(int DamageID, string description, DateTime timeOfAccupyForFix, DateTime timeOfFix, string status)
        {
            using (DataBase context = new DataBase())
            {
                var UpdateDamageBoat = (from data in context.Damages
                                        where data.DamageID == DamageID
                                        select data).Single();

                if (UpdateDamageBoat != null)
                {
                    UpdateDamageBoat.Description = description;
                    UpdateDamageBoat.Status = status;
                    UpdateDamageBoat.TimeOfOccupyForFix = timeOfAccupyForFix;
                    UpdateDamageBoat.TimeOfFix = timeOfFix;

                    if (UpdateDamageBoat.Status == "Geen schade")
                    {
                        UpdateDamageBoat.TimeOfFix = DateTime.Now;
                    }
                    context.SaveChanges();
               
                }
            }

        }
     
        // haal een specifieke boot op
        public Boat GetBoatWithName(string name)
        {

            using (var context = new DataBase())
            {
                return (from boat in context.Boats where boat.Name.Equals(name) select boat).First();
            }
        }
        // voeg een nieuwe bootdiploma toe
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

        // voeg een nieuwe bootdiploma toe
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
        // verwijder de bootdiploma
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
    }
}
