using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
   public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public DateTime? Deleted_at { get; set; }
    }
}
