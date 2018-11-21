using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootRegistratieSysteem
{
        public class BootDataBase : DbContext
        {

            public BootDataBase() : base("name = BootDataBase") { }
            public virtual DbSet<Model.User> Users { get; set; }
            //public virtual DbSet<Model.Gender> Genders { get; set; }

    }
}
