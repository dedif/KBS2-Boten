using Models;
using System.Data.Entity;

namespace BataviaReseveringsSysteem.Database
{
    public class DataBase : DbContext
    {

        public DataBase() : base("name = DataBase") { }
        public virtual DbSet<Boat> Boats { get; set; }
		public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<MemberRole> MemberRoles { get; set; }
    }
}
