using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User_Diploma
    {
        [Key,ForeignKey("User"), Column(Order =0)]
        public int UserID { get; set; }
        [Key,ForeignKey("Diploma"), Column(Order =1)]
        public int DiplomaID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User User { get; set; }
        public Diploma Diploma { get; set; }
    }
}
