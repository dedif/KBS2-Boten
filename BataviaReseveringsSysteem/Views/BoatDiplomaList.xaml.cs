using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for BoatDiplomaList.xaml
    /// </summary>
    public partial class BoatDiplomaList : UserControl
    {
        DataGrid DataGrid = new DataGrid();
        public BoatDiplomaList()
        {
            InitializeComponent();
            Load("");
        }


        private void Load(string searchtext)
        {


            using (DataBase context = new DataBase())
            {


                var boats = (from u in context.Boats where u.DeletedAt == null && u.Name.Contains(searchtext) 
                             select u).ToList();

                foreach (Boat u in boats)
                {
                    string s1 = "X";
                    string s2 = "X";
                    string s3 = "X";
                    string p1 = "X";
                    string p2 = "X";
                    string b1 = "X";
                    string b2 = "X";
                    string b3 = "X";

                    var User1Diploma = (from d in context.Boat_Diplomas
                                        where d.BoatID == u.BoatID
                                        select d.DiplomaID).ToList();

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



                    var dataUserListItems = new { u.BoatID,  u.Name,  u.Type,  S1 = s1, S2 = s2, S3 = s3, P1 = p1, P2 = p2, B1 = b1, B2 = b2, B3 = b3 };
                    DataBoatList.Items.Add(dataUserListItems);
                }



                //DataGrid.Items.Refresh();

            }
            //DataView.DataBind();
            DataGrid = DataBoatList;
        }

        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditBoatDiplomaView((int)b.Tag));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            for (int i = 0; DataBoatList.Items.Count > i;)
            {
                DataBoatList.Items.RemoveAt(i);
            }
            Load(Search.Text);

        }




    }
}