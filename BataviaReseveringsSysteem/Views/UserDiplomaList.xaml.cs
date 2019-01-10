using BataviaReseveringsSysteem.Database;
using Controllers;
using Models;
using ScreenSwitcher;
using System;
using System.Collections;
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

namespace Views
{
    /// <summary>
    /// Interaction logic for CertificateList.xaml
    /// </summary>
    public partial class UserDiplomaList : UserControl
    {
        public static DataGrid DataGrid;

        private DataBase context = new DataBase();
        private DataBaseController dbc = new DataBaseController();
        public UserDiplomaList()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            // laad de tabel in
            Load("");

        }

      

        // voeg items toe aan de tabel
        private void Load(string searchInfo)
        {



            using (DataBase cont = new DataBase())
            {


                var users = (from u in context.Users
                             where u.DeletedAt == null
                             where u.Firstname.Contains(searchInfo) || u.Lastname.Contains(searchInfo) || u.Middlename.Contains(searchInfo)
                             select u).ToList();

                foreach (User u in users)
                {
                    // default value voor diplomas
                    string s1 = "X";
                    string s2 = "X";
                    string s3 = "X";
                    string p1 = "X";
                    string p2 = "X";
                    string b1 = "X";
                    string b2 = "X";
                    string b3 = "X";

                    var User1Diploma = (from d in context.User_Diplomas
                                        where d.UserID == u.UserID
                                        select d.DiplomaID).ToList();

                    // als het id gelijks is zet dan een vinkje inplaats van een kruisje
                    if (User1Diploma.Contains(1))
                    {
                        s1 = "\u221A";
                    }


                    if (User1Diploma.Contains(2))
                    {
                        s2 = "\u221A";
                    }

                    if (User1Diploma.Contains(3))
                    {
                        s3 = "\u221A";
                    }

                    if (User1Diploma.Contains(4))
                    {
                        p1 = "\u221A";
                    }

                    if (User1Diploma.Contains(5))
                    {
                        p2 = "\u221A";
                    }

                    if (User1Diploma.Contains(6))
                    {
                        b1 = "\u221A";
                    }

                    if (User1Diploma.Contains(7))
                    {
                        b2 = "\u221A";
                    }

                    if (User1Diploma.Contains(8))
                    {
                        b3 = "\u221A";

                    }


                    // voeg toe aan de lijst
                    var dataUserListItems = new { u.UserID, Firstname = u.Firstname, Middlename = u.Middlename, Lastname = u.Lastname, S1 = s1, S2 = s2, S3 = s3, P1 = p1, P2 = p2, B1 = b1, B2 = b2, B3 = b3 };
                    DataUserList.Items.Add(dataUserListItems);
                }
            }
            DataGrid = DataUserList;
        }
        // ga naar de edit userdiploma pagina
        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditUserDiplomaView((int)b.Tag));
        }
        //zoek naar een item in de tabel
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            for (int i = 0; DataUserList.Items.Count > i;)
            {
                DataUserList.Items.RemoveAt(i);
            }
            Load(Search.Text);

        }

      
    }
}
