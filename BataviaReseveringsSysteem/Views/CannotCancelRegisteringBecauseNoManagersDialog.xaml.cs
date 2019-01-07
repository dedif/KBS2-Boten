using System.Windows;
using ScreenSwitcher;
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for CannotCancelRegisteringBecauseNoManagersDialog.xaml
    /// </summary>
    public partial class CannotCancelRegisteringBecauseNoManagersDialog
    {
        public CannotCancelRegisteringBecauseNoManagersDialog() => InitializeComponent();

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
            Close();
        }

        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}