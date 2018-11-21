using System.Windows;
using WpfApp6;

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e) => new AddBoat().Show();

        private void AfschrijvenBtn_Click(object sender, RoutedEventArgs e) => new ReserveWindow().Show();
    }
}
