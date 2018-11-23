using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BootRegistratieSysteem.Views
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : UserControl, ISwitchable
    {
        private BootDataBase context = new BootDataBase();
        private DataBaseController dbc = new DataBaseController();
        
        public EditUser(int id)
        {
            InitializeComponent();
            EditID.Content = id;

         
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
                if (Controllers.EditController.EditWithoutPassword(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, EditID))
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
                if (Controllers.EditController.Edit(Firstname, Middlename, Lastname, City, Zipcode, Address, Phonenumber, Email, Day, Month, Year, Gender, Password, ConfirmPassword, EditID))
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
