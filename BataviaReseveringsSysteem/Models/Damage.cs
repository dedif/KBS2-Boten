using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BataviaReseveringsSysteem.Models
{
    public class Damage
    {

        [Key]
        public int DamageID { get; set; }

        public Boat Boat { get; set; }
        public DateTime DateTime { get; set; }
        public User Member { get; set; }

        public Damage(Boat boat, DateTime dateTime, User member)
        {
            Boat = boat;
            DateTime = dateTime;
            Member = member;
        }

        public Damage()
        {

        }

    }
}
