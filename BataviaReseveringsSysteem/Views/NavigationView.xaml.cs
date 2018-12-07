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
            var reserveWindow = new ReserveWindow();
            Switcher.Switch(reserveWindow);
            reserveWindow.Populate();
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

        private void AssignDiplomasBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DiplomaList());
        }
    }
}
