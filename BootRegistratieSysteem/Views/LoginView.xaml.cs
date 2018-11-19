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

                        Username.BorderBrush = Brushes.Gray;
                        Password.BorderBrush = Brushes.Gray;
                        Username.BorderThickness = new Thickness(1);
                        Password.BorderThickness = new Thickness(1);
                        LoginError.Content = "";
                        LoginError.UpdateLayout();
                        Username.UpdateLayout();
                        Password.UpdateLayout();

                        if (Username.Text != "")
                        {
                            if (results.PersonID.Equals(int.Parse(Username.Text)))
                            {
                                if (Password.Password != "")
                                {
                                    if (results.Password.Equals(hashedPassword))
                                    {
                                      
                                        Switcher.Switch(new DashboardView());
                                        LoginError.UpdateLayout();
                                    }

                                    else
                                    {

                                        Password.BorderBrush = Brushes.Red;
                                        Password.BorderThickness = new Thickness(2);

                                        LoginError.Content = "Het wachtwoord komt niet met deze gebruiker overeen.";
                                        LoginError.UpdateLayout();
                                        Password.UpdateLayout();
                                    }
                                }
                            }
                            else
                            {
                                Username.BorderBrush = Brushes.Red;
                                Username.BorderThickness = new Thickness(2);

                                LoginError.Content = "Deze gebruikersnaam bestaat niet";
                                LoginError.UpdateLayout();
                                Username.UpdateLayout();
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

             
            private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
}
}
