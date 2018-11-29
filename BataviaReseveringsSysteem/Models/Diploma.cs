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
        public string DiplomaName { get; set; }
    }
}
