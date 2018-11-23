using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1;
using WpfApp6;

namespace WpfApp13
{
    public class ReservationController
    {

        public List<Reservation> GetReservations()
        {
            using (var context = new Database()) return context.Reservations.ToList();
        }

        public List<Reservation> GetReservationsForDay(DateTime day)
        {
            using (var context = new Database())
                return context.Reservations.Where(reservation =>
                    reservation.Start.Day == day.Day &&
                    reservation.Start.Month == day.Month &&
                    reservation.Start.Year == day.Year).ToList();
        }
    }
}
