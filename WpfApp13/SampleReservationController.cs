using Models;
using System;
using System.Collections.Generic;
using WpfApp13;

namespace WpfApp13
{
    public class SampleReservationController
    {
        public List<Reservation> GetReservations()
        {
            var sampleBoats = new SampleBoatController().GetBoats();
            return new List<Reservation>
            {
                new Reservation(sampleBoats[0],
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