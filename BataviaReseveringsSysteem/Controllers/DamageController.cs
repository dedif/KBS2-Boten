using System;
using BataviaReseveringsSysteem.Database;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BataviaReseveringsSysteem.Controllers
{
    public class DamageController
    {

        public DamageController()
        {

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
