using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Boat_Diploma
    {

        [Key, ForeignKey("Boat"), Column(Order = 0)]
        public int BoatID { get; set; }
        [Key, ForeignKey("Diploma"), Column(Order = 1)]
        public int DiplomaID { get; set; }
        public Boat Boat { get; set; }
        public Diploma Diploma { get; set; }

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
