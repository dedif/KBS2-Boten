using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MemberRole
    {
        [Key]
        public int MemberRoleID { get; set; }
        public int RoleID { get; set; }
        public int PersonID { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public DateTime? Deleted_at { get; set; }
    }
}
