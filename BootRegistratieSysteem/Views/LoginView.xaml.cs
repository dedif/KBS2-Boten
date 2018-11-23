using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using BootRegistratieSysteem.Views;
using BootRegistratieSysteem.Controller;

namespace BootRegistratieSysteem
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl, ISwitchable
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            if (LoginController.Login(Username, Password, LoginError))
            {
                Switcher.Switch(new UserList());
            }
            
        }

    public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

             
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
}
}
