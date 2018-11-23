using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1;
using WpfApp6;

namespace WpfApp13
{
    public class ReservationController
    {

        public List<Reservation> GetReservations()
        {
            using (Database context = new Database())
            {
                return (from db in context.Reservations select db).ToList();
            }
        }

        public List<DateTime> GetAvailableSlots(DateTime dateSelectedByCalendar)
        {
            using (var context = new Database())
            {
                return (from db in context.Reservations
                                    where
                                    dateSelectedByCalendar.Hour != db.Start.Hour
                                    && dateSelectedByCalendar.Day != db.Start.Day
                                    && dateSelectedByCalendar.Month != db.Start.Month
                                    && dateSelectedByCalendar.Year != db.Start.Year
                                    select db.Start).ToList();}
        }

        //public List<Reservation> GetReservations()
        //{
        //    var Boats = new Boatcontroller().BoatList();
        //    return new List<Reservation>
        //    {
        //        new Reservation(Boats[0],
        //            new Member(),
        //            new DateTime(2018,
        //                11,
        //                15,
        //                12,
        //                30,
        //                00),
        //            new DateTime(2018,
        //                11,
        //                15,
        //                13,
        //                00,
        //                00)),
        //    };
        //}
    }
}
