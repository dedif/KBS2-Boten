using BataviaReseveringsSysteem.Database;
using Controllers;
using Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Views;

namespace UnitTestProject2
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]

    public class UnitTestDamage
    {
        [Test]
        [TestCase("dfsefads", false)]//not empty
        [TestCase("", true)]//empty
        [Apartment(ApartmentState.STA)]

        //moet true retunen als description leeg is of whitespace
        //moet false returnen als description gevuld is

        public void IsEmpty_EmptyOrNot_ReturnBool(string description, bool result)
        {
            //Arrange
            BoatDamage boatDamage = new BoatDamage();
            //Act
            bool resultMethod = boatDamage.IsEmpty(description);
            //Assert
            Assert.AreEqual(result, resultMethod);
        }

        [Test]
        [TestCase("BootTest", " Deze boot is in de toekomst gereserveerd.")]//AlreadyReserved 
        
        public void AlreadyReserved_AlreadyReserved_ReservedIsFilled(string boatName, string answer)
        {
            //Arrange
            //maak databse
            DataBase context = new DataBase();
            //maak boot
            Boat boatTest = new Boat(boatName, Boat.BoatType.Board, 2, 2, false, DateTime.Now);
            context.Boats.Add(boatTest);
            //maak reservering met toegevoegde boot
            Reservation reservationTest = new Reservation(boatTest, DateTime.Now, DateTime.Now);
            context.Reservations.Add(reservationTest);
            //maak een damage aan bij de boot die gereserveerd is
            BoatDamage boatDamage = new BoatDamage();
            Damage damageTest = new Damage(1, 1, "test", "status");
            context.Damages.Add(damageTest);
            context.SaveChanges();
            //Act
            //boot is al gereserveerd dus reserved wordt gevuld met answer
            boatDamage.AlreadyReserved(boatName);
            // result wordt: " Deze boot is in de toekomst gereserveerd."
            string result = boatDamage.reserved;
            //Assert
            Assert.AreEqual(result, answer);
        }

        [Test]
        [TestCase]//AlreadyReserved 

        public void Notification_OneReservationAndLastLoggedInBeforeDamage_NotificationIsOne()
        {
            //Arrange
            Boat boatTest = new Boat("bootTest", Boat.BoatType.Board, 2, 2, false, DateTime.Now);
            Reservation reservationTest = new Reservation(boatTest, DateTime.Now, new DateTime());
            Damage damage = new Damage(2, boatTest.BoatID, "boot kapot", "Lichte schade");
            DateTime dateTest = new DateTime(2025, 2, 10);
            LoginController loginViewTest = new LoginController();
            DataBase context = new DataBase();

            context.Boats.Add(boatTest);
            context.Reservations.Add(reservationTest);
            context.Damages.Add(damage);

            reservationTest.Deleted = dateTest;
            damage.TimeOfClaim = dateTest;

            context.SaveChanges();
            //Act
            //Assert
            //Assert.AreEqual(result, answer);
        }
    }
}
