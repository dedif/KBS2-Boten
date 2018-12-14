using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BataviaReseveringsSysteem.Controllers
{
  public class EmailController
    {
        public  EmailController(string sendTo, string subject, string message)
        {
            SmtpClient client = new SmtpClient( );
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("Kbs2boten@gmail.com", "Windesheim1");

            MailMessage mm = new MailMessage($"donotreply@domain.com", sendTo, subject, message);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }
    }
}
