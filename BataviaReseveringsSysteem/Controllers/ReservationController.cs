using BataviaReseveringsSysteem.Database;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controllers
{
    public class ReservationController
    {

        public List<Reservation> GetReservationsForBoatThatAreNotDeleted(Boat boat)
        {
            using (var context = new DataBase())
                return context.Reservations.Where(reservation => reservation.BoatID == boat.BoatID).ToList();
        }

        public List<Reservation> GetReservationsForDayAndBoatThatAreNotDeleted(DateTime day, Boat boat)
        {
            using (var context = new DataBase())
                return context.Reservations.Where(reservation =>
                    reservation.Start.Day == day.Day &&
                    reservation.Start.Month == day.Month &&
                    reservation.Start.Year == day.Year &&
                    reservation.BoatID == boat.BoatID &&
                    reservation.Deleted == null).ToList();
        }

    }
}