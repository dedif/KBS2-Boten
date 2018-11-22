using System.Windows;
using System.Windows.Controls;
using WpfApp13;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        private void DashBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ReserveWindow());
        }
    }
}
