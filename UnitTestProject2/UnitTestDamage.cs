using NUnit.Framework;
using System.Threading;
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
