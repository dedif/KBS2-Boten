using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User_Role
    {
        [Key,ForeignKey("Role"), Column(Order =0)]
        public int RoleID { get; set; }
        [Key, ForeignKey("User"), Column(Order = 1)]
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
