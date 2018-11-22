using Models;
using System;
using System.Collections.Generic;

namespace WpfApp13
{
    class ReservationController
    {
        public List<Reservation> GetReservations()
        {
            var Boats = new Boatcontroller().BoatList();
            return new List<Reservation>
            {
                new Reservation(Boats[0],
                    new Member(),
                    new DateTime(2018,
                        11,
                        15,
                        12,
                        30,
                        00),
                    new DateTime(2018,
                        11,
                        15,
                        13,
                        00,
                        00)),
            };
        }
    }
}