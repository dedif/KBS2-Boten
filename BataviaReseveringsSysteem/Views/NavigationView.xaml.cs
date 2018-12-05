using ScreenSwitcher;
using Views;
using System.Windows;
using System.Windows.Controls;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        public NavigationView()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void MakeReservationsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ReserveWindow());
        }

        private void SeeReservationsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void ReportDamageaBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamage());
        }

        private void SeeBoatsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());
        }

        private void AddBoatsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        private void SeeUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserList());
        }

        private void AddUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }

        private void SeeDiplomasBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DiplomaList());
        }
    }
}
