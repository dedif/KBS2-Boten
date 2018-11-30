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
        [TestCase("dfsefads", false)]
        [TestCase("", true)]
        [Apartment(ApartmentState.STA)]

        public void IsEmpty_Method(string description, bool result)
        {
            //Arrange
            BoatDamage boatDamage = new BoatDamage();
            //Act
            bool resultMethod = boatDamage.IsEmpty(description);
            //Assert
            Assert.AreEqual(result, resultMethod);
        }

    }
}
