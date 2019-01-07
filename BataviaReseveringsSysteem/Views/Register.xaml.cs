using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using ScreenSwitcher;
using BataviaReseveringsSysteem.Database;
using System.Collections.Generic;
using BataviaReseveringsSysteem.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register
    {
        DataBaseController dbc = new DataBaseController();
        UserController uc = new UserController();

        public Register()
        {
            InitializeComponent();
            HorizontalAlignment = HorizontalAlignment.Center;
            using (var context = new DataBase())
            {
                // als er nog geen diploma's in de database staan maak dan deze diploma's aan.
                if (!context.Diplomas.Any(z => z.DiplomaName == "S1" || z.DiplomaName == "S2" || z.DiplomaName == "S3" || z.DiplomaName == "P1" || z.DiplomaName == "P2" || z.DiplomaName == "B1" || z.DiplomaName == "B2" || z.DiplomaName == "B3"))
                {
                    dbc.Add_Diploma("S1");
                    dbc.Add_Diploma("S2");
                    dbc.Add_Diploma("S3");
                    dbc.Add_Diploma("P1");
                    dbc.Add_Diploma("P2");
                    dbc.Add_Diploma("B1");
                    dbc.Add_Diploma("B2");
                    dbc.Add_Diploma("B3");
                }

                if (!context.Genders.Any(z => z.GenderName == "Man" || z.GenderName == "Vrouw" || z.GenderName == "Anders"))
                {
                    uc.Add_Gender("Man");
                    uc.Add_Gender("Vrouw");
                    uc.Add_Gender("Anders");

                }

                // als er nog geen rollen in de database staan maak dan deze rollen aan
                if (!context.Roles.Any(z => z.RoleName == "Reparateur" || z.RoleName == "Coach" || z.RoleName == "Wedstrijd Commissaris" || z.RoleName == "Examinator" || z.RoleName == "Bestuur"))
                {
                    dbc.Add_Role("Reparateur");
                    dbc.Add_Role("Coach");
                    dbc.Add_Role("Wedstrijd Commissaris");
                    dbc.Add_Role("Examinator");
                    dbc.Add_Role("Bestuur");
                }

                var roles = context.Roles.ToList();

                foreach (var role in roles)
                {
                    if (role.RoleName.Equals("Reparateur"))
                    {
                        Reparateur.Content = role.RoleName;
                        Reparateur.Tag = role.RoleID;
                    }
                    if (role.RoleName.Equals("Coach"))
                    {
                        Coach.Content = role.RoleName;
                        Coach.Tag = role.RoleID;
                    }
                    if (role.RoleName.Equals("Wedstrijd Commissaris"))
                    {
                        Commissaris.Content = role.RoleName;
                        Commissaris.Tag = role.RoleID;
                    }
                    if (role.RoleName.Equals("Examinator"))
                    {
                        Examinator.Content = role.RoleName;
                        Examinator.Tag = role.RoleID;
                    }
                    if (role.RoleName.Equals("Bestuur"))
                    {
                        Bestuur.Content = role.RoleName;
                        Bestuur.Tag = role.RoleID;
                    }

                }

            }
        }
        //Redirect to DashBoard
        private void Login_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        //Register user of user
        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            var managementIsChecked = Bestuur.IsChecked;
            if (managementIsChecked.HasValue && managementIsChecked.Value || new UserController().DataBaseContainsUndeletedManagementUser())
            {
                if (RegisterController.Register(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword, EndOfSubscription))
                {
                    using (var context = new DataBase())
                    {
                        var checkBoxList = new List<CheckBox> { Reparateur, Commissaris, Examinator, Coach, Bestuur };

                        foreach (var c in checkBoxList)
                        {
                            if (c.IsChecked != true) continue;
                            var roleId = int.Parse(c.Tag.ToString());
                            var userRoles = context.User_Roles.Any(x => x.RoleID == roleId && x.DeletedAt == null);
                            var lastUserId = context.Users.Select(x => x.UserID).ToList().Last();
                            var max = context.Users.OrderByDescending(p => p.UserID).FirstOrDefault().UserID;
                            dbc.Add_UserRole(roleId, max);
                        }
                    }
                    try
                    {
                        Switcher.Switch(new Dashboard());
                    }
                    catch
                    {
                        Switcher.Switch(new LoginView());
                    }

                }
                else
                {
                    RegisterError.Content = "Controleer uw gegevens!";
                    RegisterError.UpdateLayout();
                }
            }
            else
            {
                RegisterError.Content = "Je moet een bestuurslid opgeven als eerste gebruiker";
                RegisterError.UpdateLayout();
            }
        }

        // check if there are no numbers in inputbox
        public static bool IsAllLetters(string s)
        {
            //            return s.ToList().All(c => char.IsLetter(c));
            foreach (var c in s)
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
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ZipcodeValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("/^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$/i");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (uc.DataBaseContainsUndeletedManagementUser()) Switcher.Switch(new Dashboard());
            else new CannotCancelRegisteringBecauseNoManagersDialog().ShowDialog();
        }
    }
}