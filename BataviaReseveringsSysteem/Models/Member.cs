using System.ComponentModel.DataAnnotations;

namespace Models
{
    public  class Member
    {
        [Key]
        public int MemberID { get; set; }

        public Member()
        {

        }
    }
 
}
