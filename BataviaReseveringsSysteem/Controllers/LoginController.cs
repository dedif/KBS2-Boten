﻿using BataviaReseveringsSysteem.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controllers
{
    public class LoginController
    {
        // verwijder oude reserveringen
        public void DeleteOldReservations()
        {
            using (DataBase context = new DataBase())
            {
                var Reservations = (from data in context.Reservations
                                    where data.End <= DateTime.Now
                                    select data).ToList();

                foreach (var r in Reservations)
                {
                    r.Deleted = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }
        // controleer of de login gegevens kloppen en aan alle waarden voldoen
        public static Boolean IsLoginDataValid(TextBox Username, PasswordBox Password, Label LoginError)
        {
            var u = new UserController();

            using (var context = new DataBase())
            {
                var result = context.Users.Select(x => x).Where(x => x.DeletedAt == null).ToList();

               

                if (result.Count > 0)
                {
                    foreach (var results in result)
                    {

                        var hashedPassword = u.PasswordHash(Password.Password);

                        Username.BorderBrush = Brushes.Gray;
                        Password.BorderBrush = Brushes.Gray;
                        Username.BorderThickness = new Thickness(1);
                        Password.BorderThickness = new Thickness(1);
                        LoginError.Content = "";
                        LoginError.UpdateLayout();
                        Username.UpdateLayout();
                        Password.UpdateLayout();
                        if(Username.Text == "" && Password.Password == "")
                        {
                            Password.BorderBrush = Brushes.Red;
                            Password.BorderThickness = new Thickness(2);

                            Username.BorderBrush = Brushes.Red;
                            Username.BorderThickness = new Thickness(2);

                            LoginError.Content = "De gegevens komen niet overeen.";
                            LoginError.UpdateLayout();
                            Password.UpdateLayout();
                        }
                        if (Username.Text != "")
                        {
                            if (results.UserID.Equals(int.Parse(Username.Text)))
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
