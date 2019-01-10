using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Diploma
    {
        [Key]
        public int DiplomaID { get; set; }
        public string DiplomaName { get; set; }
    }
}
