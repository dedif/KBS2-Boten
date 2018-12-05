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
        public int UserID { get; set; }
        public DateTime? Deleted { get; set; }


        public User User { get; set; }

        public Reservation(Boat boat, int user, DateTime start, DateTime end)
        {
            UserID = LoginView.UserId;
            Boat = boat;
            Start = start;
            End = end;
 
        }

        public Reservation()
        {

        }
       
    }
}