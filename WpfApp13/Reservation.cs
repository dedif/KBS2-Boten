using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConsoleApp1;
using WpfApp13;

namespace WpfApp6
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Boat Boat { get; set; }
        public Member Member { get; set; }

        public Reservation(Boat boat, Member member, DateTime start, DateTime end)
        {
            Member = member;
            Boat = boat;
            Start = start;
            End = end;
        }
        public Reservation()
        {

        }
    }
}