using System;
using ConsoleApp1;

namespace WpfApp6
{
    public class Reservation
    {
        public int reservationID;
        public Boat Boat { get; set; }
        public Member Member { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Reservation(Boat boat, Member member, DateTime start, DateTime end)
        {

            Boat = boat;
            Member = member;
            Start = start;
            End = end;
        }
    }
}