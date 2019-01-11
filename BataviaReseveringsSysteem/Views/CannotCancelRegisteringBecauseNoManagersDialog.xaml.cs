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
        // Velde in het  registratiescherm leeg maken
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register()); // refresh registratie scherm
            Close();
        }
        // De applicatie afsluiten
        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        // Melding scherm afsluiten
        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}