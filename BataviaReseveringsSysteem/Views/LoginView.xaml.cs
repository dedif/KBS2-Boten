﻿using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using System.Linq;
using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;

namespace Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView
    {

        public static int UserId;
        public static User User;
        public LoginView()
        {
            InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            if (LoginController.Login(Username, Password, LoginError))
            {
                int username = int.Parse(Username.Text);
                using (DataBase context = new DataBase())
                {
                    var id = (
                        from data in context.Users
                        where data.PersonID == username
                        select data.PersonID).Single();

                    UserId = id;
                }

                Switcher.Switch(new Dashboard());
            }
            
        }
       

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }

        private void RegistrerenButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }
    }
}
