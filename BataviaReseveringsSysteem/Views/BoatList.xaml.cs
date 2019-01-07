﻿using BataviaReseveringsSysteem.Database;
using Controllers;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for BoatList.xaml
    /// </summary>
    public partial class BoatList : UserControl
    {

        public static DataGrid DataGrid;

        private DataBase context = new DataBase();
        private BoatController bc = new BoatController();

        public BoatList()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {

            //DataUserList.ItemsSource = context.Users.ToList();
            var boat = (from x in context.Boats where x.DeletedAt == null where x.Deleted == false select x).ToList();


            DataBoatList.ItemsSource = boat;


            DataGrid = DataBoatList;

        }

        private void AddBoatButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        void ButtonDelete(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;


            System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Wilt u deze boot definitief verwijderen?", "Bevestig verwijdering", System.Windows.Forms.MessageBoxButtons.YesNo, 30000);

            switch (Succes)
            {
                case System.Windows.Forms.DialogResult.No:

                    break;

                case System.Windows.Forms.DialogResult.Yes:
                    bc.SendingMail((int)b.Tag);
                    bc.DeleteBoat((int)b.Tag);
                    Switcher.Switch(new BoatList());
                    break;

            }
            
        }




        void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditBoat((int)b.Tag));

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

            DataBoatList.ItemsSource = (from x in context.Boats
                                        where (x.BoatID.ToString() == Search.Text || x.Name.Contains(Search.Text) || x.Type.ToString() == Search.Text || x.Weight.ToString() == Search.Text || x.NumberOfRowers.ToString() == Search.Text || x.Steering.ToString() == Search.Text || x.BoatLocation.ToString() == Search.Text) && x.DeletedAt == null
                                        select x).ToList();


            DataGrid = DataBoatList;
        }
    }
}
