
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserList : UserControl, ISwitchable
    {
        public static DataGrid DataGrid;
     
        private BootDataBase context = new BootDataBase();
        private DataBaseController dbc = new DataBaseController();
      
        public UserList()
        {
            InitializeComponent();
            
            Load();
          

        }

      
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

     

        private void Load()
        {

            //DataUserList.ItemsSource = context.Users.ToList();
            DataUserList.ItemsSource = (from x in context.Users
                                        where  x.Deleted_at == null
                                        select x).ToList();
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
    }
}