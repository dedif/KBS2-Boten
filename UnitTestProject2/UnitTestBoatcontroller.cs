using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using NUnit.Framework;
using WpfApp13;
using static Models.Boat;
using Assert = NUnit.Framework.Assert;

namespace UnitTest
{
    [TestFixture]

    public class UnitTestBoatcontroller
    {

        // true = De ingevulde gegevens zijn correct, false = de ingevulde gegevens zijn leeg
        [Test]
        [TestCase("Wall", "12,13", true)]
        [TestCase("  ", "14,11", false)]
        [TestCase("dino", "", false)]
        [TestCase("Shark", "33", true)]
        public void WhiteCheck_Bool(string Name, string Weight, bool answer)
        {
            //Arrange
            Boatcontroller boot = new Boatcontroller();
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
        public void WeightCheck_Bool(string Weight, bool answer)
        {
            //Arrange
            Boatcontroller boot = new Boatcontroller();
            //Act
            bool result = boot.WeightCheck(Weight);
            //Assert
            Assert.AreEqual(answer, result);
        }

      
        // true = de boot bestaat nog niet, false = de boot bestaat al
        [Test]
        [TestCase("boot", true)]
        [TestCase("pizza", false)]
       
        public void NameCheck_Bool(string name, bool answer)
        {
            //Arrange
            Boatcontroller boot = new Boatcontroller();
            boot.AddBoat("pizza", "scull", 3, 23, false);
            //Act
            bool result = boot.NameCheck(name);
            //Assert
            Assert.AreEqual(answer, result);
        }


    }
}
