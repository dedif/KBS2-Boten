using System;
using BataviaReseveringsSysteem.Database;
using Models;
using System.Collections.Generic;
using System.Linq;
using Views;
using Controllers;

namespace BataviaReseveringsSysteem.Controllers
{
    public class DamageController
    {

        public DamageController()
        {

        }

        ////Deze methode vult de gegevens van de verwijderde reservaties in
        public string ReservationContent(Reservation r)
        {
            using (DataBase context = new DataBase())
            {
                var Boat = (from data in context.Boats
                            where data.BoatID == r.BoatID
                            select data).Single();

                var Duration = r.End - r.Start;

                string content;
                content = "Naam : " + Boat.Name;
                content += "\nBegintijd: " + r.Start.Hour + ":" + r.Start.Minute;
                content += "\nDuur: " + Duration.Hours + ":" + Duration.Minutes;
                content += "\nDatum: " + r.Start.Day + "/" + r.Start.Month + "/" + r.Start.Year;
                content += "\nLocatie: " + Boat.BoatLocation;


                return content;
            }
        }

        //Deze methode stuurt een mail naar de user, die een boot had gereserveerd.
        public void SendingMail(List<Reservation> list)
        {
            using (DataBase context = new DataBase())
            {

                //Voor elke reservering die is verwijderd
                foreach (Reservation r in list)
                {
                    //De user van de reservering
                    var user = (
                    from u in context.Users
                    where u.UserID == r.UserId
                    select u).Single();

                    //Dit kijkt of er al een mail is gestuurd.
                    var AlreadySendenMails = (from m in context.Mails
                                              where m.ReservationId == r.ReservationID
                                              select m).ToList();

                    
                    if (AlreadySendenMails.Count == 0)
                    {
                        //De message van de mail
                        string sendMessage = $"Beste {user.Firstname},{Environment.NewLine}{Environment.NewLine}De boot moet vanwege zware schade worden gerepareerd.{Environment.NewLine}De onderstaande reservering is hierdoor gewijzigd:{Environment.NewLine}{ReservationContent(r)}{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}De vereniging";
                        //De titel
                        string title = "Uw reservering is gewijzigd.";
                        EmailController sendMail = new EmailController($"{user.Email}", title, sendMessage);
                        //De mail wordt verstuurd naar de gebruiker
                        Mail Mail = new Mail(r.ReservationID, title, sendMessage);
                        context.Mails.Add(Mail);
                        context.SaveChanges();
                    }
                }
            }
        }




        public List<Damage> GetDamagesList()
        {
            using (var context = new DataBase()) return context.Damages.ToList();

        }

        public bool IsThisBoatBrokenToday(Boat boat, DateTime day)
        {
            using (var context = new DataBase())
            {
                return
                    (from damage in context.Damages
                     where boat.BoatID == damage.BoatID
                     where !damage.TimeOfFix.HasValue || damage.TimeOfFix.Value.Date != day.Date
                     select damage).Any();
            }
        }

        //        public DateTime GetTimeOfClaimForBoat(Boat boat)
        //        {
        //            using (var context = new DataBase())
        //            {
        //                return (from damage in context.Damages where boat.BoatID == damage.BoatID where damage.Status == "Zwaar beschadigd" select damage.TimeOfClaim)
        //            }
        //        }
        //        public DateTime GetEarliestDamagedSlotDayQuarterForBoat(Boat boat)
        //        {
        //            using (var context = new DataBase())
        //            {
        //                return (from damage in context.Damages where boat.BoatID == damage.BoatID select damage.TimeOfClaim)
        //            }
        //        }
    }
}
