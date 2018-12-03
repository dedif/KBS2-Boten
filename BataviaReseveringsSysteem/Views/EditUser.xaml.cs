using BataviaReseveringsSysteem;
using BataviaReseveringsSysteem.Database;
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
                        where x.PersonID == id
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

                Console.WriteLine(myString);
                var split = myString.Split('-');
                Console.WriteLine(split);
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

            var MemberRoles = from x in context.MemberRoles
                              where x.PersonID == id && x.Deleted_at == null
                              select x;

            foreach (var memberRole in MemberRoles)
            {
                if (memberRole.RoleID == int.Parse(Reparateur.Tag.ToString()))
                {
                    Reparateur.IsChecked = true;
                }

                if (memberRole.RoleID == int.Parse(Coach.Tag.ToString()))
                {
                    Coach.IsChecked = true;
                }

                if (memberRole.RoleID == int.Parse(Examinator.Tag.ToString()))
                {
                    Examinator.IsChecked = true;
                }
                if (memberRole.RoleID == int.Parse(Commissaris.Tag.ToString()))
                {
                    Commissaris.IsChecked = true;
                }
                if (memberRole.RoleID == int.Parse(Administrator.Tag.ToString()))
                {
                    Administrator.IsChecked = true;
                }
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

            if (Password.Password == "" && ConfirmPassword.Password == "")
            {
                if (Controllers.EditController.EditWithoutPassword(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, EditID))
                {




                    List<int> checkBoxesTicked = new List<int>();

                    foreach (CheckBox c in RegisterLayout.Children.OfType<CheckBox>())
                    {
                        if (c.IsChecked == true)
                        {
                            int roleID = int.Parse(c.Tag.ToString());
                            //int.Parse(c.Tag.ToString());


                            var MemberRoles = context.MemberRoles.Any(x => x.RoleID == roleID && x.Deleted_at == null && x.PersonID == (int)EditID.Content);

                            if (MemberRoles)
                            {

                            } else
                            {
                                dbc.Add_MemberRole(roleID, (int)EditID.Content);
                            }



                        } else if (c.IsChecked == false)
                        {

                            int roleID = int.Parse(c.Tag.ToString());

                            var MemberRoles = context.MemberRoles.Any(x => x.RoleID == roleID && x.Deleted_at == null && x.PersonID == (int)EditID.Content);

                            if (MemberRoles)
                            {
                                dbc.Delete_MemberRole((int)EditID.Content, roleID);
                            }
                            else
                            {

                            }
                        }
                    }



                    Switcher.Switch(new UserList());
                }
                else
                {
                    RegisterError.Content = "Controleer uw gegevens!";
                    RegisterError.UpdateLayout();
                }
            }
            else
            {
                if (Controllers.EditController.Edit(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword, EditID))
                {

                    foreach (CheckBox c in RegisterLayout.Children.OfType<CheckBox>())
                    {
                        if (c.IsChecked == true)
                        {
                            int roleID = int.Parse(c.Tag.ToString());
                            //int.Parse(c.Tag.ToString());


                            var MemberRoles = context.MemberRoles.Any(x => x.RoleID == roleID && x.Deleted_at == null && x.PersonID == (int)EditID.Content);

                            if (!MemberRoles)
                            {
                            dbc.Add_MemberRole(roleID, (int)EditID.Content);
                            }
                        }
                        else if (c.IsChecked == false)
                        {

                            int roleID = int.Parse(c.Tag.ToString());

                            var MemberRoles = context.MemberRoles.Any(x => x.RoleID == roleID && x.Deleted_at == null && x.PersonID == (int)EditID.Content);

                            if (MemberRoles)
                            {
                                dbc.Delete_MemberRole((int)EditID.Content, roleID);
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
        }
    

    private void ButtonCancel(object sender, RoutedEventArgs e)
    {
        Switcher.Switch(new UserList());
    }

}

}
