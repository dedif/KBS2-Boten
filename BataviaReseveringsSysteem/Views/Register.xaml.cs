using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using ScreenSwitcher;
using BataviaReseveringsSysteem;
using BataviaReseveringsSysteem.Database;

namespace Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
	DataBaseController dbc = new DataBaseController();
        public Register()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;

            using (DataBase context = new DataBase())
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


                // als er nog geen rollen in de database staan maak dan deze rollen aan
                if (!context.Roles.Any(z => z.RoleName == "Reparateur" || z.RoleName == "Coach" || z.RoleName == "Wedstrijd Commissaris" || z.RoleName == "Examinator" || z.RoleName == "Bestuur"))
                {
                    dbc.Add_Role("Reparateur");
                    dbc.Add_Role("Coach");
                    dbc.Add_Role("Wedstrijd Commissaris");
                    dbc.Add_Role("Examinator");
                    dbc.Add_Role("Bestuur");
                }

                var Roles = context.Roles.ToList();

                foreach (var role in Roles)
                {
                    if("Reparateur" == role.RoleName)
                    {
                        Reparateur.Content = role.RoleName;
                        Reparateur.Tag = role.RoleID;
                    } 
                    if("Coach" == role.RoleName)
                    {
                        Coach.Content = role.RoleName;
                        Coach.Tag = role.RoleID;

                    }
                    if ("Wedstrijd Commissaris" == role.RoleName)
                    {
                        Commissaris.Content = role.RoleName;
                        Commissaris.Tag = role.RoleID;

                    }
                    if ("Examinator" == role.RoleName)
                    {
                        Examinator.Content = role.RoleName;
                        Examinator.Tag = role.RoleID;

                    }
                    if ("Bestuur" == role.RoleName)
                    {
                        Administrator.Content = role.RoleName;
                        Administrator.Tag = role.RoleID;

                    }
                    //Reparateur.Content = role.RoleName[2];
                    //Coach.Content = role.RoleName[3];
                    //Commissaris.Content = role.RoleName[4];
                    //Examinator.Content = role.RoleName[5];
                    //Administrator.Content = role.RoleName[6];
                }

            }
        }

        //Redirect to DashBoard
        private void Login_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
             Switcher.Switch(new LoginView());
        }

        //Register member of user
        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            if (RegisterController.Register(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword))
            {
                using (DataBase context = new DataBase()) {

                    foreach (CheckBox c in RegisterLayout.Children.OfType<CheckBox>())
                    {
                        if (c.IsChecked == true)
                        {
                            int roleID = int.Parse(c.Tag.ToString());
                            //int.Parse(c.Tag.ToString());


                            var MemberRoles = context.MemberRoles.Any(x => x.RoleID == roleID && x.Deleted_at == null);

                            var LastUserID = context.Users.Select(x => x.PersonID).ToList().Last();
                            var max = context.Users.OrderByDescending(p => p.PersonID).FirstOrDefault().PersonID;

                            dbc.Add_MemberRole(roleID, max);
                           



                        }
                    }
                }
                    Switcher.Switch(new Dashboard());
            }
            else
            {
                RegisterError.Content = "Controleer uw gegevens!";
                RegisterError.UpdateLayout();
            }
            
            

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
            Switcher.Switch(new Dashboard());
        }
    }
}
