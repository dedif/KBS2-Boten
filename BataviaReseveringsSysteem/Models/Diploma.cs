using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Diploma
    {
        [Key]
        public int DiplomaID { get; set; }
        public string Name { get; set; }

        public Diploma(int diplomaID, string name)
        {
            DiplomaID = diplomaID;
            Name = name;
        }
    }
}
