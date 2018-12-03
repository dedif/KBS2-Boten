using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controllers
{
    public class ReservationController
    {
        DataBase context = new DataBase();
        public List<Reservation> GetReservations()
        {
           return context.Reservations.ToList();
        }

        public List<Reservation> GetReservationsForDay(DateTime day)
        {
           
                return context.Reservations.Where(reservation =>
                    reservation.Start.Day == day.Day &&
                    reservation.Start.Month == day.Month &&
                    reservation.Start.Year == day.Year).ToList();
        }

        public List<Reservation> GetReservationsForDayAndBoat(DateTime day, Boat boat)
        {
            
                return context.Reservations.Where(reservation =>
                    reservation.Start.Day == day.Day &&
                    reservation.Start.Month == day.Month &&
                    reservation.Start.Year == day.Year &&
                    reservation.Boat.BoatID == boat.BoatID).ToList();
            
        }
        public List<Reservation> GetReservationsForDayAndBoatThatAreNotDeleted(DateTime day, Boat boat)
        {
          
                return context.Reservations.Where(reservation =>
                        reservation.Start.Day == day.Day &&
                        reservation.Start.Month == day.Month &&
                        reservation.Start.Year == day.Year &&
                        reservation.Boat.BoatID == boat.BoatID &&
                        reservation.Deleted == null).ToList();
            
        }
    }
}