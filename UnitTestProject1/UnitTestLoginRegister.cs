using System;
using System.Threading;
using System.Windows.Controls;
using BootRegistratieSysteem.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
 [assembly: Apartment(ApartmentState.STA)]
namespace UnitTestLoginRegister 
{ 

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class UnitTestLoginRegister
    {

        [Test]
        [Apartment(ApartmentState.STA)]
        [TestCase("1", "Omar", false)]
        [TestCase("1", "unittest", true)]

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

        //Werkt nog niet helemaal goed, combobox is nog even irritant
        //[Test]
        //[Apartment(ApartmentState.STA)]
        //[TestCase("Omar", "Omar", "Omar", "Omar", "Omar", "Omar", "5555", "Omar","45","12","2458", 0, "Omar", "Omar", false)]
        //public void Register_Register_ReturnTrue(string Firstname,
        //                             string Middlename,
        //                             string Lastname,
        //                             string City,
        //                             string Zipcode,
        //                             string Address,
        //                             string Phonenumber,
        //                             string Email,
        //                             string Day,
        //                             string Month,
        //                             string Year,
        //                             int Gender,
        //                             string Password,
        //                             string ConfirmPassword,
        //                             bool answer)
        //{
        //    //Arrage
        //    TextBox F = new TextBox() { Text = Firstname };
        //    TextBox M = new TextBox() { Text = Middlename };
        //    TextBox L = new TextBox() { Text = Lastname };
        //    TextBox C = new TextBox() { Text = City };
        //    TextBox Z = new TextBox() { Text = Zipcode };
        //    TextBox A = new TextBox() { Text = Address };
        //    TextBox P = new TextBox() { Text = Phonenumber };
        //    TextBox E = new TextBox() { Text = Email };
        //    TextBox D = new TextBox() { Text = Day };
        //    TextBox Mo = new TextBox() { Text = Month };
        //    TextBox Y = new TextBox() { Text = Year };

        //    ComboBox G = new ComboBox() {Name = "",SelectedIndex = 0,SelectedValue= 0 };
        //    G.Items.Add("");
        //    G.SelectedItem = 0;

            
        //    PasswordBox Pa = new PasswordBox() { Password = Password };
        //    PasswordBox Co = new PasswordBox() { Password = ConfirmPassword };

        //    //Act
        //    bool result = RegisterController.Registreren(F, M, L, C, Z, A, P, E, D, Mo, Y, G, Pa, Co);

        //    //Assert
        //    Assert.AreEqual(answer, result);
        //}


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
