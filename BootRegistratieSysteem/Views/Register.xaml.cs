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

namespace BootRegistratieSysteem
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl,ISwitchable
    {
        public Register()
        {
            this.InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Login_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            DataBaseController u = new DataBaseController();

            string savedPasswordHash = u.PasswordHash(Password.Password);
            
            u.Add_User(savedPasswordHash, Firstname.Text, Lastname.Text, Address.Text, Zipcode.Text, City.Text, Phonenumber.Text, Email.Text);
        }
    }
}
