﻿using BataviaReseveringsSysteem.Controllers;
using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Views;

namespace Controllers
{
    public class DashboardController
    {

        DataBase context = new DataBase();
        Dashboard Dashboard;

        public DashboardController(Dashboard dashboard)
        {
            Dashboard = dashboard;
            UserController.CheckSubscription();
        }
        //deze methode kijkt of wanneer je het laatst bent ingelogd en geeft een melding wanneer je reserveringen zijn veranderd
        public void Notification(DateTime lastLogged)
        {
            var DamagedBoatsOfUser = (from data in context.Damages
                                      join a in context.Reservations

                                      on data.BoatID equals a.BoatID

                                      where data.TimeOfClaim > lastLogged
                                      where data.TimeOfClaim == a.Deleted
                                      where a.Deleted != null
                                      where data.TimeOfFix == null
                                      where data.Status == "Zware schade"
                                      select data).ToList();

            var User = (from data in context.Users

                        where data.UserID == LoginView.UserId

                        select data).Single();


            if (DamagedBoatsOfUser.Count > 1)
            {
                //melding met meerdere veranderingen
                MessageBoxResult Notification = MessageBox.Show(
                                 "Uw reserveringen zijn gewijzigd omdat de boot uit de vaart is genomen",
                                 "Melding",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Information);


            }

            if (DamagedBoatsOfUser.Count == 1)
            {
                MessageBoxResult Notification = MessageBox.Show(
                           //melding met 1 verandering
                           "Uw reservering is gewijzigd omdat de boot uit de vaart is genomen",
                           "Melding",
                           MessageBoxButton.OK,
                           MessageBoxImage.Information);

            }
            //last login wordt geupdate na het melden van schade
            User.LastLoggedIn = DateTime.Now;
            context.SaveChanges();




        }




        //Deze methode vult de labels van de huidige reservaties
        public string ReservationContent(Reservation reservation)
        {
            using (DataBase context = new DataBase())
            {

                var ReservationBoatID = (
                    from r in context.Reservations
                    where r.ReservationID == reservation.ReservationID
                    select r.BoatID).Single();

                var Name =
                    (from boat in context.Boats
                     where boat.BoatID == ReservationBoatID
                     select boat.Name).Single();

                var StartDate =
                  (from r in context.Reservations
                   where r.ReservationID == reservation.ReservationID
                   select r.Start).Single();

                var EndDate =
                  (from r in context.Reservations
                   where r.ReservationID == reservation.ReservationID
                   select r.End).Single();

                var Duration = EndDate - StartDate;
                string DurationMinuteString = Duration.Minutes.ToString();

                var BoatLocation =
                 (from r in context.Reservations
                  where r.ReservationID == reservation.ReservationID
                  select r.Boat.BoatLocation).Single();

                string Minutes = StartDate.Minute.ToString();

                if (StartDate.Minute < 10)
                {
                    Minutes = "0" + Minutes;
                }
                if (Duration.Minutes < 10)
                {
                    DurationMinuteString = "0" + DurationMinuteString;
                }

                string content;
                content = "Naam : " + Name;
                content += "\nBegintijd: " + StartDate.Hour + ":" + Minutes;
                content += "\nDuur: " + Duration.Hours + ":" + DurationMinuteString;
                content += "\nDatum: " + StartDate.Day + "/" + StartDate.Month + "/" + StartDate.Year;
                content += "\nLocatie: " + BoatLocation;

                return content;
            }
        }

        //Deze methode verwijderd de bijbehorende reservatie
        public void DeleteReservation(int id, bool competition, bool coach)
        {
            using (DataBase context = new DataBase())
            {
                var delete = (
                    from r in context.Reservations
                    where r.ReservationID == id
                    select r).Single();
                //De gebruiker krijgt een controle melding.
                var confirm = MessageBox.Show(
                                "Weet u zeker dat u de volgende afschrijving wilt verwijderen:\n"
                                + ReservationContent(delete),
                                "Melding",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information);

                //Als de gebruiker de reservering wilt verwijderen.
                if (confirm != MessageBoxResult.Yes) return;
                //De reservering wordt uit de database verwijderd. 
                delete.Deleted = DateTime.Now;
                context.SaveChanges();
                //Alle oude knoppen en labels worden verwijderd van het scherm.
                Dashboard.DeleteAllControls();
                //De nieuwe reserveringen worden op het scherm getoond. 
                Dashboard.ShowReservations(competition, coach);

            }
        }
        //Deze methode verwijderd alle controls


        public Button AddDeleteButton(int x, int y, int id)
        {

            Button Right = new Button()
            {
                //Er wordt een button aangemaakt. 
                Content = "Afschrijving annuleren",
                Margin = new Thickness(x, y, 0, 0),
                Height = 30,
                FontSize = 16,
                Tag = id,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            //De button krijgt een click event
            Right.Click += Dashboard.DeleteButton_Click;


            return Right;
        }

    }
}
