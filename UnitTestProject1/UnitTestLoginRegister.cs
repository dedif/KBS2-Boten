using System;
using System.Windows.Controls;
using BootRegistratieSysteem.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
namespace UnitTestLoginRegister
{
   

    [TestFixture]
    public class UnitTestLoginRegister
    {

        [STAThread]
        [TestCase("10", "Omar", true)]
        [TestCase("1", "Omar", false)]

        public void Login_Login_ReturnTrue(string username, string password, bool answer)
        {
            //Arrage
            TextBox T = new TextBox() { Text = username };
            PasswordBox P = new PasswordBox() { Password = password };
            LoginController login = new LoginController();
            //Act
            bool result = LoginController.Login(T, P, new Label());
            //Assert
            Assert.AreEqual(answer, result);
        }


        [STAThread]
        [TestCase("", "", "", "", "", "", "", "",0,0,0, "", "", "", false)]
        public void Register_Register_ReturnTrue(
                                     string Firstname,
                                     string Middlename,
                                     string Lastname,
                                     string City,
                                     string Zipcode,
                                     string Address,
                                     string Phonenumber,
                                     string Email,
                                     int Day,
                                     int Month,
                                     int Year,
                                     string Gender,
                                     string Password,
                                     string ConfirmPassword,
                                     bool answer)
        {
            //Arrage
            TextBox F = new TextBox() { Text = Firstname };
            TextBox M = new TextBox() { Text = Middlename };
            TextBox L = new TextBox() { Text = Lastname };
            TextBox C = new TextBox() { Text = City };
            TextBox Z = new TextBox() { Text = Zipcode };
            TextBox A = new TextBox() { Text = Address };
            TextBox P = new TextBox() { Text = Phonenumber };
            TextBox E = new TextBox() { Text = Email };
            TextBox D = new TextBox() { Text = Day.ToString() };
            TextBox Mo = new TextBox() { Text = Month.ToString() };
            TextBox Y = new TextBox() { Text = Year.ToString() };
            ComboBox G = new ComboBox() { Text = Gender };
            PasswordBox Pa = new PasswordBox() { Password = Password };
            PasswordBox Co = new PasswordBox() { Password = ConfirmPassword };

            //Act
            bool result = RegisterController.Registreren(F, M, L, C, Z, A, P, E, D, Mo, Y, G, Pa, Co);

            //Assert
            Assert.AreEqual(answer, result);
        }


        [Test]
        [TestCase("",true)]
        [TestCase("sdfsdfsdf",true)]
        [TestCase("123dsfgsgsdf",false)]
        [TestCase("TestPersoon",true)]
        public void Register_AllLetters_ReturnTrue(string x,bool answer)
        {
            //Arrage

            //Act
            bool result = RegisterController.IsAllLetters(x);
            //Assert
            Assert.AreEqual(answer,result);
        
        }
        // Testen of er een email 
        [Test]
        [TestCase("545454", false)]
        [TestCase("sdfsdfsdf", false)]
        [TestCase("@live.nl", false)]
        [TestCase("TestPersoo@live.nl", true)]
        public void IsEmail_Email_ReturnTrue(string x, bool answer)
        {
            //Arrage

            //Act
            bool result = RegisterController.IsEmailValid(x);
            //Assert
            Assert.AreEqual(answer, result);

        }


    }
}
