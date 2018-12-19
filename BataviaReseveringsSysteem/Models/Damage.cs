using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Damage
    {
        [Key]
        public int DamageID { get; set; }
        [ForeignKey("User"), Column(Order =0)]
        public int UserID { get; set; }
        [ForeignKey("Boat"), Column(Order = 1)]
        public int BoatID { get; set; }
        public string Description { get; set; }
        public DateTime TimeOfClaim { get; set; }
        public DateTime? TimeOfOccupyForFix { get; set; }
        public DateTime? TimeOfFix { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Boat Boat { get; set; }
        public Damage(int userID, int boatID, string description, string status)
        {

            UserID = userID;
            BoatID = boatID;
            Description = description;
            TimeOfClaim = DateTime.Now;
            Status = status;
        }

        public Damage()
        {

        }

    }
}
