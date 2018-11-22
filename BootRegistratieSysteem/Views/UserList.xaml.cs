
﻿using System;
using System.Collections.Generic;
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
      
        public UserList()
        {
            InitializeComponent();

            Load();
          

        }

        private void Search_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            DataUserList.ItemsSource = from x in context.Users
                        where x.Firstname == search.Text || x.Lastname == search.Text || x.City == search.Text || x.Address == search.Text || x.Zipcode == search.Text || x.Phonenumber == search.Text || x.Email == search.Text
                        select x;


            DataGrid = DataUserList;
        }
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

     

        private void Load()
        {

            DataUserList.ItemsSource = context.Users.ToList();
            DataGrid = DataUserList;


        }

        private void RegisterUserButton(object sender, RoutedEventArgs e)
        {

        }



        void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditUser((int)b.Tag));
        }

      
    }
}