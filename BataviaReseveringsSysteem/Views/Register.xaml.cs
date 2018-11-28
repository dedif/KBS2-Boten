using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Controllers;
using ScreenSwitcher;

namespace Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            this.InitializeComponent();

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
