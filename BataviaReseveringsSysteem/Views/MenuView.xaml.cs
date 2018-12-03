using System.Windows;
using System.Windows.Controls;
using ScreenSwitcher;
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void AfschrijvingDoen_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ReserveWindow());
        }

        private void AfschrijvingenInzien_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void BotenBekijken_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());
        }

        private void BotenToevoegen_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }
    }
}
