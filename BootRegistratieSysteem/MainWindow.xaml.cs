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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace BootRegistratieSysteem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Byte[] salt;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            new RNGCryptoServiceProvider().GetBytes(salt = new Byte[16]);
            //var passwordSalt = new Rfc2988DeriveBytes();
            DataBaseController u = new DataBaseController();
            //b.EmptyDatabase();
            
            u.Add_User(Password.Password, Firstname.Text, Lastname.Text, Address.Text, Zipcode.Text, City.Text, Phonenumber.Text, Email.Text);
        }
    }
}
