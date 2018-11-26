using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Views.Controller;
using WpfApp13;

namespace Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            if (LoginController.Login(Username, Password, LoginError))
            {
                Switcher.Switch(new Dashboard());
            }
            
        }

        
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
}
}
