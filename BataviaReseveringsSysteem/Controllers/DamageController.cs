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

    }
}
