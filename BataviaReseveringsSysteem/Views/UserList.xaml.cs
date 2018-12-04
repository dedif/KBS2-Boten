using BataviaReseveringsSysteem.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScreenSwitcher;
using BataviaReseveringsSysteem;
using BataviaReseveringsSysteem.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        public static DataGrid DataGrid;

        private DataBase context = new DataBase();
        private DataBaseController dbc = new DataBaseController();

        public UserList()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;

            Load();


        }


        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }



        private void Load()
        {

            //DataUserList.ItemsSource = context.Users.ToList();
            var user = (from x in context.Users
                        where x.Deleted_at == null
                        select x).ToList();


            DataUserList.ItemsSource = user;


            DataGrid = DataUserList;

        }

        private void RegisterUserButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }

        void ButtonDelete(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            dbc.Delete_User((int)b.Tag);
            Switcher.Switch(new UserList());
        }




        void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditUser((int)b.Tag));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

            DataUserList.ItemsSource = (from x in context.Users
                                        where x.PersonID.ToString() == Search.Text || x.Firstname.Contains(Search.Text) || x.Lastname.Contains(Search.Text) || x.City.Contains(Search.Text) || x.Address.Contains(Search.Text) || x.City.Contains(Search.Text) || x.Zipcode.Contains(Search.Text) || x.Email.Contains(Search.Text) || x.Phonenumber.Contains(Search.Text) || x.Birthday.Day.ToString() == Search.Text || x.Birthday.Month.ToString() == Search.Text || x.Birthday.Year.ToString() == Search.Text && x.Deleted_at == null
                                        select x).ToList();


            DataGrid = DataUserList;
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void Diploma(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DiplomaList());

        }

        private void Boat(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());

        }
    }
}