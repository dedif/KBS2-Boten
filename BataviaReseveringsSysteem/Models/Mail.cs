using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Mail
    { 
        [Key,ForeignKey("Reservation")]
        public int ReservationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime IsSent { get; set; }
        public User User { get; set; }
        public Reservation Reservation { get; set; }
        public Mail(int reservationId, string title, string message)
        {
            ReservationId = reservationId;
            Title = title;
            Message = message;
            IsSent = DateTime.Now;


        }

        public Mail()
        {

        }
    }
}
