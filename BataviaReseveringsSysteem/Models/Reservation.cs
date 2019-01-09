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
        public bool Competition { get; set; } = false;
        public bool Coach { get; set; } = false;
        public DateTime? Deleted { get; set; }
        public User User { get; set; }
        public Boat Boat { get; set; }
        public Reservation(Boat boat, bool competition,bool coach, DateTime start, DateTime end)
        {
            Coach = coach;
            Competition = competition;
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