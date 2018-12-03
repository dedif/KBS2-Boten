using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Boat
    {
        public enum BoatType {Scull, Skiff, Board}

        [Key]
        public int BoatID { get; set; }
        public string Name { get; set; }
        public BoatType Type { get; set; }
        public int NumberOfRowers { get; set;}
        public double Weight { get; set; }
        public bool Steering { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Boat(string name, BoatType type, int numberOfRowers, double weight, bool steering, DateTime createdAt)
        {
            Name = name;
            Type = type;
            NumberOfRowers = numberOfRowers;
            Weight = weight;
            Steering = steering;
            CreatedAt = createdAt;


        }
        public Boat()
        {

        }




    }
}
