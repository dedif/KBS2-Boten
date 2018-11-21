
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

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Load()
        {

            DataUserList.ItemsSource = context.Users.ToList();
            DataGrid = DataUserList;

        }

        private void RegisterUserButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserList());
        }
    }
}