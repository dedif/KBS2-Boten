using BataviaReseveringsSysteem;
using BataviaReseveringsSysteem.Database;
using Controllers;
using ScreenSwitcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Views
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : UserControl
    {
        private DataBase context = new DataBase();
        private DataBaseController dbc = new DataBaseController();
        public EditUser(int id)
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            EditID.Content = id;


            var users = from x in context.Users
                        where x.UserID == id
                        select x;

            foreach (var user in users)
            {
                Firstname.Text = user.Firstname;
                Lastname.Text = user.Lastname;
                Middlename.Text = user.Middlename;
                City.Text = user.City;
                Address.Text = user.Address;
                Zipcode.Text = user.Zipcode;
                Email.Text = user.Email;
                Phonenumber.Text = user.Phonenumber;
                Gender.SelectedIndex = user.GenderID - 1;

                string myString = user.Birthday.ToString("dd-MM-yyyy"); // From Database
           
                var split = myString.Split('-');
                
                Year.Text = split[2];
                Month.Text = split[1];
                Day.Text = split[0];
            }

            var Roles = context.Roles.ToList();

            foreach (var role in Roles)
            {
                switch (role.RoleName)
                {
                    case "Reparateur":
                        Reparateur.Content = role.RoleName;
                        Reparateur.Tag = role.RoleID;
                        break;
                    case "Coach":
                        Coach.Content = role.RoleName;
                        Coach.Tag = role.RoleID;
                        break;
                    case "Wedstrijd Commissaris":
                        Commissaris.Content = role.RoleName;
                        Commissaris.Tag = role.RoleID;
                        break;
                    case "Examinator":
                        Examinator.Content = role.RoleName;
                        Examinator.Tag = role.RoleID;
                        break;
                    case "Bestuur":
                        Administrator.Content = role.RoleName;
                        Administrator.Tag = role.RoleID;
                        break;
                }
                
            }

            var User_Roles = from x in context.User_Roles
                              where x.UserID == id && x.DeletedAt == null
                              select x.RoleID;

          

            foreach (var userRole in User_Roles)
            {
                if (userRole == int.Parse(Reparateur.Tag.ToString()))
                {
                    Reparateur.IsChecked = true;
                }

                if (userRole == int.Parse(Coach.Tag.ToString()))
                {
                    Coach.IsChecked = true;
                }

                if (userRole == int.Parse(Examinator.Tag.ToString()))
                {
                    Examinator.IsChecked = true;
                }
                if (userRole == int.Parse(Commissaris.Tag.ToString()))
                {
                    Commissaris.IsChecked = true;
                }
                if (userRole == int.Parse(Administrator.Tag.ToString()))
                {
                    Administrator.IsChecked = true;
                }

              
            }

            // check de rollen van de ingelogte gebruiker en zet rollen op hidden als de gebruiker de rechten niet heeft
            var Login_User_Role = from x in context.User_Roles
                                  where x.UserID == LoginView.UserId && x.DeletedAt == null
                                  select x.RoleID;

            if (!Login_User_Role.Contains(5))
                {
                    Reparateur.Visibility = Visibility.Hidden;
                    Coach.Visibility = Visibility.Hidden;
                    Examinator.Visibility = Visibility.Hidden;
                    Commissaris.Visibility = Visibility.Hidden;
                    Administrator.Visibility = Visibility.Hidden;
                }
            
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        // check if there are no numbers in inputbox
        public static bool IsAllLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;

        }
        // Check if there is are no letters in input box
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ZipcodeValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("/^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$/i");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            
                if (Controllers.EditController.Edit(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword, EditID))
                {

                    foreach (CheckBox c in RegisterLayout.Children.OfType<CheckBox>())
                    {
                        if (c.IsChecked == true)
                        {
                            int roleID = int.Parse(c.Tag.ToString());
                            //int.Parse(c.Tag.ToString());


                            var User_Roles = context.User_Roles.Any(x => x.RoleID == roleID && x.DeletedAt == null && x.UserID == (int)EditID.Content);

                             if (!User_Roles)
                            {
                            dbc.Add_UserRole(roleID, (int)EditID.Content);

                            }
                           



                        }
                        else if (c.IsChecked == false)
                        {

                            int roleID = int.Parse(c.Tag.ToString());

                            var User_Roles = context.User_Roles.Any(x => x.RoleID == roleID && x.DeletedAt == null && x.UserID == (int)EditID.Content);

                            if (User_Roles)
                            {
                                dbc.Delete_UserRole((int)EditID.Content, roleID);
                            }
                            else
                            {

                            }
                        }
                        Switcher.Switch(new UserList());
                    }
                }
                else
                {
                    RegisterError.Content = "Controleer uw gegevens!";
                    RegisterError.UpdateLayout();
                }
            
        }
    

    private void ButtonCancel(object sender, RoutedEventArgs e)
    {
        Switcher.Switch(new UserList());
    }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserList());
        }
    }
    

}
