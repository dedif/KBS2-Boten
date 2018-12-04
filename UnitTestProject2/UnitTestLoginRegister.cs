using Controllers;
using NUnit.Framework;
using System.Threading;
using System.Windows.Controls;
using Assert = NUnit.Framework.Assert;
using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem;

namespace UnitTestLoginRegister
{

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class UnitTestLoginRegister
    {

        [Test]
        [Apartment(ApartmentState.STA)]
        [TestCase("Omar", "Omar", "Omar", "Omar", "Omar", "Omar", "5555", "Omar", "45", "12", "2458", 0, "Omar", "Omar", false)]
        [TestCase("", "", "", "", "", "", "", "", "00", "00", "0000", 0, "", "", false)]
        [TestCase("Omar", "A", "Mazher", "Zwolle", "8023XW", "Westeinde22", "1234567890", "tester@live.nl", "05", "12", "1995", 0, "omar", "omar", true)]
        public void Register_Register_ReturnTrue(string Firstname,
                                     string Middlename,
                                     string Lastname,
                                     string City,
                                     string Zipcode,
                                     string Address,
                                     string Phonenumber,
                                     string Email,
                                     string Day,
                                     string Month,
                                     string Year,
                                     int Gender,
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
            TextBox D = new TextBox() { Text = Day };
            TextBox Mo = new TextBox() { Text = Month };
            TextBox Y = new TextBox() { Text = Year };

            ComboBox G = new ComboBox() { Name = "", SelectedIndex = 0, SelectedValue = 0 };
            G.Items.Add(new ComboBoxItem() { Content = "", Tag = 0 });
            G.SelectedItem = 0;


            PasswordBox Pa = new PasswordBox() { Password = Password };
            PasswordBox Co = new PasswordBox() { Password = ConfirmPassword };

            //Act
            bool result = RegisterController.Register(F, M, L, C, Z, A, P, E, D, Mo, Y, G, Pa, Co);

            //Assert
            Assert.AreEqual(answer, result);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        [TestCase("2", "Omar", false)]
        [TestCase("1", "omar", true)]

        public void Login_Login_ReturnTrue(string username, string password, bool answer)
        {
            //Arrage
            TextBox T = new TextBox() { Text = username };
            PasswordBox P = new PasswordBox() { Password = password };
            LoginController login = new LoginController();
            //Act
            bool result = LoginController.IsLoginDataValid(T, P, new Label());
            //Assert
            Assert.AreEqual(answer, result);
        }


        [Test]
        [TestCase("", true)]
        [TestCase("sdfsdfsdf", true)]
        [TestCase("123dsfgsgsdf", false)]
        [TestCase("TestPersoon", true)]
        public void Register_AllLetters_ReturnTrue(string x, bool answer)
        {
            //Arrage

            //Act
            bool result = RegisterController.DoesletterOnlyTextboxContainNumber(x);
            //Assert
            Assert.AreEqual(answer, result);

        }
        // Validatie email
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

        [Test]
        [TestCase(1,1,true)]
        [TestCase(8,1,false)]
        public void AddMemberRole_MemberRole_Void(int role,int personID,bool answer)
        {
            //Arrage
            DataBaseController dbC = new DataBaseController();
            DataBase Db = new DataBase();
            //Act
            dbC.Add_MemberRole(role, personID);
            bool result = dbC.Get_MemberRole(role, personID);
            //Assert
            Assert.AreEqual(answer, result);
        }
    }
}
