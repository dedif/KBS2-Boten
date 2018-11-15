using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Database : DbContext
    {

        public Database() : base("name = BootDataBase") { }
        public virtual DbSet<Boat> Boats { get; set; }

    }
}
