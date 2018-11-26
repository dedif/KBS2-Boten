using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Boat Boat { get; set; }
        public Member Member { get; set; }
        public Boolean Deleted { get; set; } = false;

        public Reservation(Boat boat, Member member, DateTime start, DateTime end)
        {
            Member = member;
            Boat = boat;
            Start = start;
            End = end;
 
        }
        public Reservation()
        {

        }
    }
}