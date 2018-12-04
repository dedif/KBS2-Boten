using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using System.Linq;
using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using BataviaReseveringsSysteem;

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
                                where data.Deleted == null
                                select data.Boat).ToList();
                if (ReservationInfo.Count > 0)
                {
                    DataReservations.Visibility = Visibility.Visible;
                }
                DataReservations.ItemsSource = ReservationInfo;
            }
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            if (LoginController.IsLoginDataValid(Username, Password, LoginError))
            {
                int username = int.Parse(Username.Text);
                using (DataBase context = new DataBase())
                {
                    var member = (
                        from data in context.Users
                        where data.PersonID == username
                        select data.PersonID).Single();

                    UserId = member;

                }

                Switcher.Switch(new Dashboard());
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
