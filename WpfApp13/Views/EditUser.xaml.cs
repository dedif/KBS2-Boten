using Controllers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp13;

namespace Views
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : UserControl
    {
        private Database context = new Database();
        public EditUser(int id)
        {
            InitializeComponent();
            test.Content = id;

         
            var users = from x in context.Users
                        where x.PersonID == id
                        select x;
                
            foreach(var user in users)
            {
                Firstname.Text = user.Firstname;
                Lastname.Text = user.Lastname;
                City.Text = user.City;
                Address.Text = user.Address;
                Zipcode.Text = user.Zipcode;
                Email.Text = user.Email;
                Phonenumber.Text = user.Phonenumber;
                Gender.SelectedIndex = user.GenderID-1;
    
                string myString = user.Birthday.ToString("dd-MM-yyyy"); // From Database
                Console.WriteLine(myString);
                var split = myString.Split('-');
                Console.WriteLine(split);
                 Year.Text = split[2];
                 Month.Text = split[1];
                 Day.Text =  split[0];

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
                if (EditController.EditWithoutPassword(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, test))
                {
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
                if (EditController.Edit(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword, test))
                {
                    Switcher.Switch(new UserList());
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
