﻿using System;
using Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using NUnit.Framework;
using static Models.Boat;
using Assert = NUnit.Framework.Assert;

namespace UnitTest
{
    [TestFixture]

    public class UnitTestBoatcontroller
    {

        // true = De ingevulde gegevens zijn correct, false = de ingevulde gegevens zijn leeg
        [Test]
        [TestCase("Wall", "12,13", true)]//return true omdat er geen whitespace is
        [TestCase("  ", "14,11", false)]//return false omdat er whitespace is
        [TestCase("dino", "", false)]//return false omdat er whitespace is
        [TestCase("Shark", "33", true)]//return true omdat er geen whitespace is

        public void WhiteCheck_WithOrWithoutWhiteSpace_ReturnBool(string Name, string Weight, bool answer)
        {
            //Arrange
            BoatController boot = new BoatController();
            //Act
            bool result = boot.WhiteCheck(Name, Weight);
            //Assert
            Assert.AreEqual(answer, result);
        }

        //true = het gewicht is een int, false = het gewicht is niet correct ingevoerd
        [Test]
        [TestCase("4,66", true)]
        [TestCase(" ", false)]
        [TestCase("numbers", false)]
        [TestCase("", false)]
        [TestCase("33", true)]
        public void WeightCheck_WeightFilledWithNumbersOrNot_ReturnBool(string Weight, bool answer)
        {
            //Arrange
            BoatController boot = new BoatController();
            //Act
            bool result = boot.WeightCheck(Weight);
            //Assert
            Assert.AreEqual(answer, result);
        }

      
        // true = de boot bestaat nog niet, false = de boot bestaat al
        [Test]
        [TestCase("boot", true)]
        [TestCase("pizza", false)]
       
        public void NameCheck_NameContainsOrNot_ReturnBool(string name, bool answer)
        {
            //Arrange
            BoatController boot = new BoatController();
            boot.AddBoat("pizza", "scull", 3, 23, false);
            //Act
            bool result = boot.NameCheck(name);
            //Assert
            Assert.AreEqual(answer, result);
        }


    }
}
