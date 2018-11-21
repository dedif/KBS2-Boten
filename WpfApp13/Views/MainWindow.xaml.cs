using System.Windows;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddBoat addBoat = new AddBoat();
            addBoat.Show();



        }

        private void DashBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Dashboard b = new Dashboard();
            b.Show();
        }

        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {
            ReserveWindow r = new ReserveWindow();
            r.Show();
        }
    }
}
