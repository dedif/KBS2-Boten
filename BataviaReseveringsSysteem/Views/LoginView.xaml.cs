using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using System.Linq;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System;

namespace Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView
    {

        public static int UserId;
        LoginController loginController = new LoginController();

        public LoginView()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            loginController.DeleteOldReservations();
            using (DataBase context = new DataBase())
            {
                var ReservationInfo = (from data in context.Reservations
                                       where data.Deleted == null
                                       orderby data.Start
                                       select data).ToList();

                var BoatInfo = (from data in context.Reservations
                                join boats in context.Boats on data.BoatID equals boats.BoatID
                                where data.Deleted == null
                                select boats).ToList();


                if (ReservationInfo.Count > 0)
                {
                    DataReservations.Visibility = Visibility.Visible;
                }
                DataReservations.ItemsSource = ReservationInfo;
            }
        }

        public void LoginButton(object sender, RoutedEventArgs e)
        {
            if (LoginController.IsLoginDataValid(Username, Password, LoginError))
            {
                int username = int.Parse(Username.Text);
                using (DataBase context = new DataBase())
                {
                    var user = (
                        from data in context.Users
                        where data.UserID == username 
                        select data).Single();

                    UserId = user.UserID;
                    Switcher.MenuMaker();
                    Switcher.Switch(new Dashboard());
                    user.LastLoggedIn = DateTime.Now;
                    context.SaveChanges();
                }
            }

        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void RegistrerenButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }

    }
}