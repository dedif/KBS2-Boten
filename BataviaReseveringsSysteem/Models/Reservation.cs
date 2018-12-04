using System;
using System.ComponentModel.DataAnnotations;
using Views;

namespace Models
{
    public class Reservation
    {
        //primaire sleutel voor de database
        [Key]
        public int ReservationID { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Boat Boat { get; set; }
        public int UserId { get; set; }
        public DateTime? Deleted { get; set; }

        public Reservation(Boat boat, DateTime start, DateTime end)
        {
            UserId = LoginView.UserId;
            Boat = boat;
            Start = start;
            End = end;
 
        }

        public Reservation()
        {

        }
       
    }
}