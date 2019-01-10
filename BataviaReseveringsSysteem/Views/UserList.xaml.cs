using BataviaReseveringsSysteem.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScreenSwitcher;
using BataviaReseveringsSysteem;
using BataviaReseveringsSysteem.Views;
using Controllers;
using Models;



namespace Views
{
    /// <summary>
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        public static DataGrid DataGrid;

        
        private UserController usc = new UserController();

        public UserList()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            // laad de tabel in
            Load();
            using (DataBase context = new DataBase())
            {
                var rol = (from data in context.User_Roles
                           where data.UserID == LoginView.UserId
                           select data.RoleID).ToList();
            }
        }
        // voeg items toe aan de tabel
        private void Load()
        {
            using (DataBase context = new DataBase())
            {
                var users = (from u in context.Users join g in context.Genders on u.GenderID equals g.GenderID
                             where u.DeletedAt == null && u.UserID != LoginView.UserId
                             select new {u.UserID,Firstname = u.Firstname, Middlename = u.Middlename, Lastname = u.Lastname, Gender = g.GenderName , Birthday = u.Birthday, City = u.City, Address = u.Address, Zipcode = u.Zipcode, Phonenumber = u.Phonenumber, Email = u.Email }).ToList();

                DataUserList.ItemsSource = users;
              }

            DataGrid = DataUserList;
        }
        //ga naar de register pagina om een nieuwe user aante maken
        private void RegisterUserButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }
        //verwijder het geselecteerde item
        void ButtonDelete(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Wilt u deze gebruiker definitief verwijderen?", "Bevestig verwijdering", System.Windows.Forms.MessageBoxButtons.YesNo, 30000);

            switch (Succes)
            {
                case System.Windows.Forms.DialogResult.No:
                    
                    break;

                case System.Windows.Forms.DialogResult.Yes:
                    usc.Delete_User((int)b.Tag);


                    Switcher.Switch(new UserList());
                    break;

            }
        }
        // ga naar de edit user pagina
        void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditUser((int)b.Tag));
        }
        //zoek naar een item in de tabel
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (DataBase context = new DataBase())
            {
                DataUserList.ItemsSource = (from x in context.Users join g in context.Genders on x.GenderID equals g.GenderID
                                            where x.UserID.ToString() == Search.Text || x.Firstname.Contains(Search.Text) || x.Lastname.Contains(Search.Text) || x.City.Contains(Search.Text) || x.Address.Contains(Search.Text) || x.City.Contains(Search.Text) || x.Zipcode.Contains(Search.Text) || x.Email.Contains(Search.Text) || x.Phonenumber.Contains(Search.Text) || x.Birthday.Day.ToString() == Search.Text || x.Birthday.Month.ToString() == Search.Text || x.Birthday.Year.ToString() == Search.Text || g.GenderName.Contains(Search.Text) && x.DeletedAt == null
                                            select new { x.UserID, Firstname = x.Firstname, Middlename = x.Middlename, Lastname = x.Lastname, Gender = g.GenderName, Birthday = x.Birthday, City = x.City, Address = x.Address, Zipcode = x.Zipcode, Phonenumber = x.Phonenumber, Email = x.Email }).ToList();



                DataGrid = DataUserList;
            }
        }
    }
}