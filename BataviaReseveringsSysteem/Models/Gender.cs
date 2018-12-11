using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Gender
    {
        [Key]
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        
    }
}
