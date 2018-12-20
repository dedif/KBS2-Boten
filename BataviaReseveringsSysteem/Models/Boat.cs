using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Boat
    {
        public enum BoatType { Scull = 0, Skiff = 1, Board = 2 }

        [Key]
        public int BoatID { get; set; }

        public string Name { get; set; }
        public BoatType Type { get; set; }
        public int NumberOfRowers { get; set; }
        public double Weight { get; set; }
        public bool Steering { get; set; }
        [Index(IsUnique = true)]
        public int BoatLocation { get; set; }
        public DateTime AvailableAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Broken { get; set; } = false;
        public bool Deleted { get; set; }

        public Boat(string name, BoatType type, int numberOfRowers, double weight, bool steering, int boatLocation, DateTime createdAt, DateTime availableAt)
        {
            Name = name;
            Type = type;
            NumberOfRowers = numberOfRowers;
            Weight = weight;
            Steering = steering;
            BoatLocation = boatLocation;
            CreatedAt = createdAt;
            AvailableAt = availableAt;
        }

        public Boat()
        {
        }
    }
}
