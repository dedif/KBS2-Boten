using System.ComponentModel.DataAnnotations;

namespace Model
{
    class Gender
    {
        [Key]
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        
    }
}
