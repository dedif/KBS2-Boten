using BataviaReseveringsSysteem.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controllers
{
    public class LoginController
    {

        public static Boolean IsLoginDataValid(TextBox Username,PasswordBox Password,Label LoginError){

            UserController u = new UserController();

            using (DataBase context = new DataBase())
            {
                var Result = context.Users.ToList();
               
                
                if (Result.Count > 0)
                {
                    foreach (var results in Result)
                    {
                        
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
                                       return true;
                                    }

                                    else
                                    {

                                        Password.BorderBrush = Brushes.Red;
                                        Password.BorderThickness = new Thickness(2);

                                        LoginError.Content = "De gegevens komen niet overeen.";
                                        Username.BorderBrush = Brushes.Red;
                                        Username.BorderThickness = new Thickness(2);
                                        LoginError.UpdateLayout();
                                        Password.UpdateLayout();
                                        return false;
                                    }
                                }
                                else
                                {
                                    Password.BorderBrush = Brushes.Red;
                                    Password.BorderThickness = new Thickness(2);
                                    LoginError.Content = "Vul een wachtwoord in";
                                    LoginError.UpdateLayout();
                                    Password.UpdateLayout();
                                    return false;
                                }
                            }
                            else
                            {
                                Password.BorderBrush = Brushes.Red;
                                Password.BorderThickness = new Thickness(2);

                                Username.BorderBrush = Brushes.Red;
                                Username.BorderThickness = new Thickness(2);

                                LoginError.Content = "De gegevens komen niet overeen.";
                                LoginError.UpdateLayout();
                                Password.UpdateLayout();
                             
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show("De gegevens komen niet overeen.");
                    return false;
                }

            }
            return false;

        }
    }
}
