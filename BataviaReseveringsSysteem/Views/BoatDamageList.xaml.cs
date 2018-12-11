using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System;
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
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for BoatDamageList.xaml
    /// </summary>
    public partial class BoatDamageList : UserControl
    {
        DataGrid DataGrid = new DataGrid();
        public BoatDamageList()
        {
            InitializeComponent();
            Load();
        }

        private void Load( )
        {
            using (DataBase context = new DataBase())
            {


                DataBoatDamageList.ItemsSource = (from b in context.Boats
                             join d in context.Damages on b.BoatID equals d.BoatID
                             join u in context.Users on d.UserID equals u.UserID
                             where b.DeletedAt == null
                             select new { b.BoatID, BoatName = b.Name, TimeOfClaim = d.TimeOfClaim, TimeOfFix = d.TimeOfFix, Description = d.Description, Status = d.Status, FirstName = u.Firstname, LastName = u.Lastname, MiddleName = u.Middlename }).ToList();


                DataGrid = DataBoatDamageList;
                }
        
        }

        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditBoatDamage((int)b.Tag));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (DataBase context = new DataBase())
            {
                DataBoatDamageList.ItemsSource = (from x in context.Boats join d in context.Damages on x.BoatID equals d.BoatID
                                                  join u in context.Users on d.UserID equals u.UserID
                                                  where x.BoatID.ToString() == Search.Text || x.Name.Contains(Search.Text) || d.TimeOfClaim.ToString() == Search.Text || d.TimeOfFix.ToString() == Search.Text || d.Description.Contains(Search.Text) || d.Status.Contains(Search.Text) || u.Firstname.Contains(Search.Text)  || u.Middlename.Contains(Search.Text) || u.Lastname.Contains(Search.Text) && x.DeletedAt == null
                                                  select new { x.BoatID, BoatName = x.Name, TimeOfClaim = d.TimeOfClaim, TimeOfFix = d.TimeOfFix, Description = d.Description, Status = d.Status, FirstName = u.Firstname, MiddleName = u.Middlename, LastName = u.Lastname }).ToList();


                DataGrid = DataBoatDamageList;
            }

        }

    }
}
