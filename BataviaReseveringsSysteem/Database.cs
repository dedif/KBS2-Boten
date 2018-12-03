using BataviaReseveringsSysteem.Models;
using Models;
using System.Data.Entity;

namespace BataviaReseveringsSysteemDatabase
{
    public class Database : DbContext
    {

        public Database() : base("name = DataBase") { }
        public virtual DbSet<Boat> Boats { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Damage> Damages { get; set; }




    }
}
