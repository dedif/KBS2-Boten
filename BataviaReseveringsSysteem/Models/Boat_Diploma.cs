using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Boat_Diploma
    {

        [Key]
        public int BoatDiplomaID { get; set; }
        public int BoatID { get; set; }
        public int DiplomaID { get; set; }

        public Boat_Diploma(int boatID, int diplomaID)
        {
            BoatID = boatID;
            DiplomaID = diplomaID;
        }

        public Boat_Diploma()
        {

        }
    }
}
