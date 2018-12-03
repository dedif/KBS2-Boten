using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using System.Linq;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using Models;
using System;

namespace Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {

        public static int LoggedUserID;
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
                                       where data.Deleted == null
                                       select data.Boat).ToList();
                if (ReservationInfo.Count > 0)
                {
                    DataReservations.Visibility = Visibility.Visible;
                }
                DataReservations.ItemsSource = ReservationInfo;
                

             //   BoatName.Binding = BoatInfo;
            }
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            if (LoginController.Login(Username, Password, LoginError))
            {
                int username = int.Parse(Username.Text);
                using (DataBase context = new DataBase())
                {
                    var User = (
                        from data in context.Users
                        where data.PersonID == username
                        select data).Single();

                    LoggedUserID = User.PersonID;


                    Dashboard s = new Dashboard();
                    Switcher.Switch(s);
                    //de lastLoggedIn wordt veranderd als je inlogt
                    User.LastLoggedIn = DateTime.Now;
                    context.SaveChanges();
                        }
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
