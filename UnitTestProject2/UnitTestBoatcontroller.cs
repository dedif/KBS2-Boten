using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using WpfApp13;
using Assert = NUnit.Framework.Assert;

namespace UnitTest
{
    [TestFixture]

    public class UnitTestBoatcontroller
    {

        [Test]

        [TestCase("Wall", "12,13", true)]
        [TestCase("  ", "14,11", false)]
        [TestCase("dino", "", false)]
        [TestCase("Shark", "33", true)]
        public void WhiteCheck_ReturnsFalseWhenNameFieldIsBlank(string Name, string Weight, bool answer)
        {
            //Arrange
            Boatcontroller boot = new Boatcontroller();
            //Act
            bool result = boot.WhiteCheck(Name, Weight);
            //Assert
            Assert.AreEqual(answer, result);
        }

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

    }
}
