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
        public int EditID;
        public EditUser(int id)
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            EditID = id;
            UserID.Content = id;


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
                EndOfSubscription.SelectedDate = user.EndOfSubscription;

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
                        Bestuur.Content = role.RoleName;
                        Bestuur.Tag = role.RoleID;
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
                if (userRole == int.Parse(Bestuur.Tag.ToString()))
                {
                    Bestuur.IsChecked = true;
                }


            }

            // check de rollen van de ingelogte gebruiker en zet rollen op hidden als de gebruiker de rechten niet heeft
            var Login_User_Role = from x in context.User_Roles
                                  where x.UserID == LoginView.UserId && x.DeletedAt == null
                                  select x.RoleID;

            if (!Login_User_Role.Contains(5))
            {
                AccountLabel.Visibility = Visibility.Hidden;
                Reparateur.Visibility = Visibility.Hidden;
                Coach.Visibility = Visibility.Hidden;
                Examinator.Visibility = Visibility.Hidden;
                Commissaris.Visibility = Visibility.Hidden;
                Bestuur.Visibility = Visibility.Hidden;
                SubscriptionLabel.Visibility = Visibility.Hidden;
                EndOfSubscription.Visibility = Visibility.Hidden;
                VerwijderenBtn.Visibility = Visibility.Hidden;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            var Login_User_Role = from x in context.User_Roles
                                  where x.UserID == LoginView.UserId && x.DeletedAt == null
                                  select x.RoleID;
            if (Login_User_Role.Contains(5))
            {
                Switcher.Switch(new UserList());
            }
            else
            {
                Switcher.Switch(new Dashboard());
            }
        }
        private void VerwijderenBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze gebuiker wil verwijderen?",
                                          "Waarschuwing",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DataBaseController.Delete_User(EditID);
                Switcher.Switch(new UserList());
            }
        }

        private void BewerkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Controllers.EditController.Edit(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword, UserID, EndOfSubscription))
            {
                List<CheckBox> CheckBoxList = new List<CheckBox>() { Reparateur, Coach, Commissaris, Examinator, Bestuur };
                foreach (CheckBox c in CheckBoxList)
                {
                    if (c.IsChecked == true)
                    {
                        int roleID = int.Parse(c.Tag.ToString());
                        //int.Parse(c.Tag.ToString());


                        var User_Roles = context.User_Roles.Any(x => x.RoleID == roleID && x.DeletedAt == null && x.UserID == EditID);

                        if (!User_Roles)
                        {

                            dbc.Add_UserRole(roleID, EditID);
                        }




                    }
                    else if (c.IsChecked == false)
                    {

                        int roleID = int.Parse(c.Tag.ToString());

                        var User_Roles = context.User_Roles.Any(x => x.RoleID == roleID && x.DeletedAt == null && x.UserID == EditID);

                        if (User_Roles)
                        {
                            dbc.Delete_UserRole(EditID, roleID);
                        }
                        else
                        {

                        }
                    }


                    //als de rechten van de gebruiker bestuur is, ga dan naar de userlist. Als je geen bestuur bent ga je terug naar het dashboard.
                    var Login_User_Role = from x in context.User_Roles
                                          where x.UserID == LoginView.UserId && x.DeletedAt == null
                                          select x.RoleID;
                    if (Login_User_Role.Contains(5))
                    {
                        Switcher.Switch(new UserList());
                    }
                    else
                    {
                        Switcher.Switch(new Dashboard());
                    }

                }
            }
            else
            {
                RegisterError.Content = "Controleer uw gegevens!";
                RegisterError.UpdateLayout();
            }
        }
    }


}