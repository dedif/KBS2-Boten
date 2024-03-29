﻿using Views;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ScreenSwitcher;
using BataviaReseveringsSysteem.Database;
using Models;
using System.Linq;

namespace Controllers
{
    public class EditController
    {
        public static Boolean Edit(TextBox Firstname,
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
                                    PasswordBox ConfirmPassword,
                                    Label editID,
                                    DatePicker EndOfSub)
        {
            bool validate = true;
            bool valDate = true;
            bool hasPassword = true;
            string savedPasswordHash = "";
            DateTime dt = new DateTime();
            TextBox[] controls = { Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year };

            foreach (var item in controls)
            {

                item.BorderBrush = Brushes.Gray;
                item.BorderThickness = new Thickness(1);


                if (item.Text == "" && item.Name != "Middlename")
                {
                    TextBoxAlert(item);
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
                        TextBoxAlert(item);
                        validate = false;
                    }
                }
            }
            // update password layout 
            Password.BorderBrush = Brushes.Gray;
            Password.BorderThickness = new Thickness(1);
            ConfirmPassword.BorderBrush = Brushes.Gray;
            ConfirmPassword.BorderThickness = new Thickness(1);
            EndOfSub.BorderBrush = Brushes.Gray;
            EndOfSub.BorderThickness = new Thickness(1);

            // check if passwords are filled 
            if (Password.Password == "" && ConfirmPassword.Password == "")
            {
                hasPassword = false;
            }
            if (Password.Password == "" && hasPassword)
            {
                AlertPassword(Password);
                validate = false;
            }
            if (ConfirmPassword.Password == "" && hasPassword)
            {
                AlertPassword(ConfirmPassword);
                validate = false;
            }
            if (EndOfSub.SelectedDate != null)
            {
                if (EndOfSub.SelectedDate.Value < DateTime.Now)
                {
                    EndOfSub.BorderBrush = Brushes.Red;
                    EndOfSub.BorderThickness = new Thickness(2);
                    EndOfSub.UpdateLayout();
                    validate = false;
                }

            }
            UserController u = new UserController(); // Get database
            if (hasPassword)
            {
                savedPasswordHash = u.PasswordHash(Password.Password); // Hash password

            }
            string BirthdayText = $"{ConvertDate(Day.Text)}-{ConvertDate(Month.Text)}-{Year.Text}";

            //localtime
            DateTime DateTimeToday = DateTime.UtcNow.Date;
            string DateToday = DateTimeToday.ToString("dd-MM-yyyy");

            // validate date
            try
            {
                if (valDate && ((int.Parse(Day.Text) > 31) || (int.Parse(Month.Text) > 12) || (int.Parse(Year.Text) < 1900) || (int.Parse(Year.Text) > int.Parse(DateTime.Today.Year.ToString()))))
                {
                    TextBoxAlert(Day);
                    TextBoxAlert(Month);
                    TextBoxAlert(Year);

                    valDate = false;
                }
            }
            catch (FormatException)
            {
                TextBoxAlert(Day);
                TextBoxAlert(Month);
                TextBoxAlert(Year);
                valDate = false;
            }

            int GenderID = int.Parse(((ComboBoxItem)Gender.SelectedItem).Tag.ToString());

            // email validation
            if (Email.Text != "" && !IsEmailValid(Email.Text))
            {
                validate = false;
                TextBoxAlert(Email);
            }



            if (valDate && validate)
            {
                dt = DateTime.ParseExact(BirthdayText, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }


            // validate passwords        

            if (!Password.Password.Equals(ConfirmPassword.Password) && hasPassword)
            {
                AlertPassword(Password);
                AlertPassword(ConfirmPassword);
                validate = false;
            }

            // add user to database
            if (validate && valDate && hasPassword)
            {
                sendNewSubscription(Firstname, Email, EndOfSub, (int)editID.Content);
                u.Update_User((int)editID.Content, savedPasswordHash, Firstname.Text, Middlename.Text, Lastname.Text, Address.Text, Zipcode.Text, City.Text, Phonenumber.Text, Email.Text, GenderID, dt, EndOfSub.SelectedDate);
                MessageBoxResult result = MessageBox.Show("Het account is bijgewerkt.");
                Switcher.Switch(new Views.UserList());
                return true;
            }
            if (validate && valDate && !hasPassword)
            {
                sendNewSubscription(Firstname, Email, EndOfSub, (int)editID.Content);
                u.Update_User((int)editID.Content, Firstname.Text, Middlename.Text, Lastname.Text, Address.Text, Zipcode.Text, City.Text, Phonenumber.Text, Email.Text, GenderID, dt, EndOfSub.SelectedDate);
                Switcher.DeleteMenu();
                Switcher.MenuMaker();
                MessageBoxResult result = MessageBox.Show("Het account is bijgewerkt.");
                Switcher.Switch(new Views.UserList());
                return true;
            }
            else
            {
                return false;
            }

        }
        // wijzig het lidmaatschap van de user aan
        public static void sendNewSubscription(TextBox firstname, TextBox email, DatePicker endDate, int userID)
        {
            using (DataBase context = new DataBase())
            {
                User update = context.Users.Where(d => d.UserID == userID).First();

                if (update.EndOfSubscription != endDate.SelectedDate)
                {
                    string sendMessage = $"Beste {firstname.Text},{Environment.NewLine}{Environment.NewLine}Lidnummer:{userID}{Environment.NewLine}{Environment.NewLine}Uw lidmaatschap is gewijzigd, uw lidmaatschap loopt nu tot {endDate.SelectedDate.Value.ToString("dd-MM-yyyy")}.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}De Roeivereniging";
                    EmailController mail = new EmailController(email.Text, "Lidmaatschap gegevens gewijzigd", sendMessage);
                }
            }

        }

        // Melding voor passwordboxs
        public static void AlertPassword(PasswordBox P)
        {
            P.BorderBrush = Brushes.Red;
            P.BorderThickness = new Thickness(2);
            P.UpdateLayout();
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

        //Email validatie
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
        //Enkel digit veranderen naar double digit
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
        //Melding voor Textboxes
        public static void TextBoxAlert(TextBox T)
        {
            T.BorderBrush = Brushes.Red;
            T.BorderThickness = new Thickness(2);
            T.UpdateLayout();
        }

    }
}