using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ConsoleApp1;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject2
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void WhiteCheck_Bool()
        {
            //Arrange
            Boatcontroller boot = new Boatcontroller();
            //Act
            bool result = boot.WhiteCheck("goed", "12,32");
            //Assert
            Assert.AreEqual(true, result);
        }
    }
}
