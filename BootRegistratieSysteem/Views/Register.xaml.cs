using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using BootRegistratieSysteem.Views;
using System.Globalization;
using System.Net.Mail;

namespace BootRegistratieSysteem
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl, ISwitchable
    {
        public Register()
        {
            this.InitializeComponent();

        }

        //Redirect to DashBoard
        private void Login_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
             Switcher.Switch(new LoginView());
        }

        //Register member of user
        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            bool validate = true;
            bool valDate = true;
            DateTime dt = new DateTime();
            TextBox[] controls = { Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year };

            foreach (var item in controls)
            {

                item.BorderBrush = Brushes.Gray;
                item.BorderThickness = new Thickness(1);


                if (item.Text == "" && item.Name != "Middlename")
                {
                    item.BorderBrush = Brushes.Red;
                    item.BorderThickness = new Thickness(2);
                    validate = false;
                    if (item.Name == "Day" || item.Name == "Month" || item.Name == "Year")
                    {
                        valDate = false;
                    }
                }
                if (item.Name == "Firstname" || item.Name == "Lastname" || item.Name == "Middlename" || item.Name == "City")
                {
                    if (!IsAllLetters(item.Text))
                    {
                        item.BorderBrush = Brushes.Red;
                        item.BorderThickness = new Thickness(2);
                        item.UpdateLayout();
                        validate = false;
                    }
                }
            }
            // update password layout 
            Password.BorderBrush = Brushes.Gray;
            Password.BorderThickness = new Thickness(1);
            ConfirmPassword.BorderBrush = Brushes.Gray;
            ConfirmPassword.BorderThickness = new Thickness(1);
            // check if passwords are filled 
            if (Password.Password == "")
            {
                Password.BorderBrush = Brushes.Red;
                Password.BorderThickness = new Thickness(2);
                validate = false;
            }
            if (ConfirmPassword.Password == "")
            {
                ConfirmPassword.BorderBrush = Brushes.Red;
                ConfirmPassword.BorderThickness = new Thickness(2);
                validate = false;
            }

            DataBaseController u = new DataBaseController(); // Get database

            string savedPasswordHash = u.PasswordHash(Password.Password); // Hash password
            string BirthdayText = $"{Day.Text}-{Month.Text}-{Year.Text}";

            //localtime
            DateTime DateTimeToday = DateTime.UtcNow.Date;
            string DateToday = DateTimeToday.ToString("dd-MM-yyyy");

            // validate date
            if (valDate && ((int.Parse(Day.Text) > 31) || (int.Parse(Month.Text) > 12) || (int.Parse(Year.Text) > int.Parse(DateTime.Today.Year.ToString()))))
            {
                Day.BorderBrush = Brushes.Red;
                Day.BorderThickness = new Thickness(2);
                Day.UpdateLayout();
                Month.BorderBrush = Brushes.Red;
                Month.BorderThickness = new Thickness(2);
                Month.UpdateLayout();
                Year.BorderBrush = Brushes.Red;
                Year.BorderThickness = new Thickness(2);
                Year.UpdateLayout();

                valDate = false;
            }


            int GenderID = int.Parse(((ComboBoxItem)this.Gender.SelectedItem).Tag.ToString());

            // email validation
            if (Email.Text != "" && !IsEmailValid(Email.Text))
            {
                validate = false;
                Email.BorderBrush = Brushes.Red;
                Email.BorderThickness = new Thickness(2);
                Email.UpdateLayout();
            }
            

            if (valDate && validate)
            {
                dt = DateTime.ParseExact(BirthdayText, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }


            // validate passwords
            if (Password.Password == "")
            {
                Password.BorderBrush = Brushes.Red;
                Password.BorderThickness = new Thickness(2);
                Password.UpdateLayout();
                validate = false;
            }

            if (ConfirmPassword.Password == "")
            {
                ConfirmPassword.BorderBrush = Brushes.Red;
                ConfirmPassword.BorderThickness = new Thickness(2);
                ConfirmPassword.UpdateLayout();
                validate = false;
            }

            if (!Password.Password.Equals(ConfirmPassword.Password))
            {
                Password.BorderBrush = Brushes.Red;
                Password.BorderThickness = new Thickness(2);
                ConfirmPassword.BorderBrush = Brushes.Red;
                ConfirmPassword.BorderThickness = new Thickness(2);
                Password.UpdateLayout();
                ConfirmPassword.UpdateLayout();
                validate = false;
            }

            // add user to database
            if (validate && valDate)
            {

                MessageBoxResult result = MessageBox.Show("UW ACCOUNT IS AANGEMAAKT!!!!");
                u.Add_User(savedPasswordHash, Firstname.Text, Lastname.Text, Address.Text, Zipcode.Text, City.Text, Phonenumber.Text, Email.Text, GenderID, dt);
                Switcher.Switch(new DashboardView());

            }
            else
            {
                RegisterError.Content = "Controleer uw gegevens!";
                RegisterError.UpdateLayout();
            }

        }
        // check if there are no numbers in inputbox
        public static bool IsAllLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;

        }
        // Check if there is are no letters in input box
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public bool IsEmailValid(string emailaddress)
        {
            try
            {

                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // ISwitchable utilizes this
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();

        }

    }
}
