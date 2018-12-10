using Models;
using System.Data.Entity;

namespace BataviaReseveringsSysteem.Database
{
    public class DataBase : DbContext
    {

        public DataBase() : base("name = DataBase") { }
        public virtual DbSet<Boat> Boats { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Diploma> Diplomas { get; set; }
        public virtual DbSet<User_Diploma> User_Diplomas { get; set; }
        public virtual DbSet<Boat_Diploma> Boat_Diplomas { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<User_Role> User_Roles { get; set; }
        public virtual DbSet<Damage> Damages { get; set; }
    }
}
    

