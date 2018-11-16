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
    public partial class LoginView : UserControl, ISwitchable
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            DataBaseController u = new DataBaseController();
            
            using (BootDataBase context = new BootDataBase())
            {
                var Result = context.Users.ToList();
                if (Result.Count > 0)
                {
                    foreach (var results in Result)
                    {
                        Console.WriteLine(results);
                        string hashedPassword = u.PasswordHash(Password.Password);

                        if (results.PersonID.Equals(int.Parse(Username.Text)))
                        {
                            if (results.Password.Equals(hashedPassword))
                            {
                                MessageBox.Show("Wachtwoord klopt");
                            }
                            else
                            {
                                MessageBox.Show("Wachtwoord klopt niet");
                            }
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("Lijst is leeg");
                }

            }

        }       
        
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
    }
}
