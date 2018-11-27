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

        }

        //Redirect to DashBoard
        private void Login_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
             Switcher.Switch(new LoginView());
        }

        //Register member of user
        private void ButtonRegister(object sender, RoutedEventArgs e)
        {
            if (RegisterController.Registreren(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword))
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
