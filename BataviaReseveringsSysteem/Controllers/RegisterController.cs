using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Controllers;

namespace Controllers
{


    public class RegisterController
    {

        //Register user of user

        public static Boolean Register(TextBox Firstname, 
                                     TextBox Middlename, 
                                     TextBox Lastname, 
                                     TextBox City,
                                     TextBox Zipcode,
                                     TextBox Address,
                                     TextBox Phonenumber,
                                     TextBox Email,
                                     TextBox Day,
                                     TextBox Month,
                                     TextBox Year,
                                     ComboBox Gender,
                                     PasswordBox Password,
                                     PasswordBox ConfirmPassword)
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
                    ErrorAlert(item);
                    validate = false;
                    if (item.Name == "Day" || item.Name == "Month" || item.Name == "Year")
                    {
                        valDate = false;
                    }
                }
                if (item.Name == "Firstname" || item.Name == "Lastname" || item.Name == "Middlename" || item.Name == "City")
                {
                    if (!DoesletterOnlyTextboxContainNumber(item.Text))
                    {
                        ErrorAlert(item);
                        validate = false;
                    }
                }
            }
            // update password layout 
            Password.BorderBrush = Brushes.Gray;
            Password.BorderThickness = new Thickness(1);
            ConfirmPassword.BorderBrush = Brushes.Gray;
            ConfirmPassword.BorderThickness = new Thickness(1);

            //update gender layout
            Gender.BorderBrush = Brushes.Gray;
            Gender.BorderThickness = new Thickness(1);
  

            // check if passwords are filled 
            if (Password.Password == "")
            {
                ErrorAlertPassword(Password);
                validate = false;
            }
            if (ConfirmPassword.Password == "")
            {
                ErrorAlertPassword(ConfirmPassword);
                validate = false;
            }

            UserController u = new UserController(); // Get database

            string savedPasswordHash = u.PasswordHash(Password.Password); // Hash password
            string BirthdayText = $"{ConvertDate(Day.Text)}-{ConvertDate(Month.Text)}-{Year.Text}";

            //localtime
            DateTime DateTimeToday = DateTime.UtcNow.Date;
            string DateToday = DateTimeToday.ToString("dd-MM-yyyy");

            // validate date
            try
            {
                if (valDate && ((int.Parse(Day.Text) > 31) || (int.Parse(Month.Text) > 12) || (int.Parse(Year.Text) < 1900) || (int.Parse(Year.Text) > int.Parse(DateTime.Today.Year.ToString()))))
                {
                    ErrorAlert(Day);
                    ErrorAlert(Month);
                    ErrorAlert(Year);
                    valDate = false;
                }
            }
            catch (FormatException)
            {
                ErrorAlert(Day);
                ErrorAlert(Month);
                ErrorAlert(Year);
                valDate = false;
            }


            // email validation
            if (Email.Text != "" && !IsEmailValid(Email.Text))
            { 
                ErrorAlert(Email);
                validate = false;
                
            }


            if (valDate && validate)
            {
                dt = DateTime.ParseExact(BirthdayText, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            // Checked of gender is geselecteerd
              if (Gender.SelectedIndex == -1)
            {
                ErrorAlertGender(Gender);
                validate = false;
            }
           

            // validate passwords
            if (Password.Password == "")
            {
                ErrorAlertPassword(Password);
                validate = false;
            }

            if (ConfirmPassword.Password == "")
            {
                ErrorAlertPassword(ConfirmPassword);
                validate = false;
            }

            if (!Password.Password.Equals(ConfirmPassword.Password))
            {
                ErrorAlertPassword(Password);
                ErrorAlertPassword(ConfirmPassword);
                validate = false;
            }

            // add user to database
            if (validate && valDate)
            {
                int GenderID = int.Parse(((ComboBoxItem)Gender.SelectedItem).Tag.ToString());

                u.Add_User(savedPasswordHash, Firstname.Text, Middlename.Text, Lastname.Text, Address.Text, Zipcode.Text, City.Text, Phonenumber.Text, Email.Text, GenderID, dt);
                MessageBoxResult result = MessageBox.Show("Het account is aangemaakt, het lidnummer is " + u.GetID());
                return true;
            }
            else
            {
                return false;
            }
           
        }
        // check if there are no numbers in inputbox
        public static bool DoesletterOnlyTextboxContainNumber(string s)
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
        

        public static bool IsEmailValid(string emailaddress)
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
        public static string ConvertDate(string x)
        {


            if (x.Length < 2)
            {
                string z = "0";
                z += x;
                return z;
            }

            return x;
        }

        public static void ErrorAlert(TextBox T)
        {
            T.BorderBrush = Brushes.Red;
            T.BorderThickness = new Thickness(2);
            T.UpdateLayout();
        }

        public static void ErrorAlertGender(ComboBox C)
        {
            C.BorderBrush = Brushes.Red;
            C.BorderThickness = new Thickness(2);
            C.UpdateLayout();
        }
        public static void ErrorAlertPassword(PasswordBox P)
        {
            P.BorderBrush = Brushes.Red;
            P.BorderThickness = new Thickness(2);
            P.UpdateLayout();
        }
    }
}
