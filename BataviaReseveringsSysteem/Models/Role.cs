using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
   public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
