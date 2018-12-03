﻿using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BataviaReseveringsSysteem.Models
{
    public class Damage
    {

        [Key]
        public int DamageID { get; set; }

        public Boat Boat { get; set; }
        public DateTime DateTime { get; set; }
        public User Member { get; set; }

        public Damage(Boat boat, DateTime dateTime, User member)
        {
            Boat = boat;
            DateTime = dateTime;
            Member = member;
        }


namespace Models
{
   public class Damage
    {
        [Key]
        public int MemberID { get; set; }
        public int BoatID { get; set; }
        public string Description { get; set; }
        public DateTime TimeOfClaim { get; set; }
        public DateTime? TimeOfFix { get; set; }
        public string Status { get; set; }

        public Damage(int memberID, int boatID, string description, string status)
        {

            MemberID = memberID;
            BoatID = boatID;
            Description = description;
            TimeOfClaim = DateTime.Now;
            Status = status;
        }

        public Damage()
        {

        }

    }
}
