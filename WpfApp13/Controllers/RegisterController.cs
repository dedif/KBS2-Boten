﻿using System;
using System.Globalization;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Controllers;

namespace Controllers
{
    public class RegisterController
    {
        //Register member of user
        public static Boolean Registreren(TextBox Firstname, 
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

            UserController u = new UserController(); // Get database

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


            int GenderID = int.Parse(((ComboBoxItem)Gender.SelectedItem).Tag.ToString());

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
                
                return true;
            }
            else
            {
                return false;
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
    }
}