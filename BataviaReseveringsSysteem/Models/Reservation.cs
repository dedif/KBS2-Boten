using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Boat")]
        public int BoatID { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime? Deleted { get; set; }
      
        public Reservation(Boat boat, DateTime start, DateTime end)
        {
            UserId = LoginView.UserId;
            BoatID = boat.BoatID;
            Start = start;
            End = end;
 
        }

        public Reservation()
        {

        }
       
    }
}