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

namespace BootRegistratieSysteem
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }


        private void LoginButton(object sender, RoutedEventArgs e)
        {
            DataBaseController u = new DataBaseController();
            u.GetUser();

            using (BootDataBase context = new BootDataBase())
            {
                var Result = context.Users.ToList();

                foreach(var results in Result)
                {

                    if (Result.Count > 0)
                    {
                        if (results.Firstname == Username.Text)
                        {


                            string PasswordHash = results.Password;
                            byte[] hashBytes = Convert.FromBase64String(PasswordHash);
                            byte[] salt = new byte[16];
                            Array.Copy(hashBytes, 0, salt, 0, 16);
                            var pbkdf2 = new Rfc2898DeriveBytes(Password.Password, salt, 10000);
                            byte[] hash = pbkdf2.GetBytes(20);

                            int Ok = 1;
                            for (int i = 0; i < 20; i++)
                            {
                                if (hashBytes[i + 16] != hash[i])

                                    Ok = 0;


                                if (Ok == 1)
                                {
                                    MessageBox.Show("Wachtwoord klopt");
                                }
                                else
                                {
                                    MessageBox.Show("Wachtwoord klopt niet");
                                }
                            }

                        } else
                        {
                            MessageBox.Show("Uw voornaam bestaat niet");
                        }
                    }
                   
                }

            }

              
        }

        
    }
}
