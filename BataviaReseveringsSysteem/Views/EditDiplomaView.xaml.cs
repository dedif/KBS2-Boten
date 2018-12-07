using BataviaReseveringsSysteem.Database;
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

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for EditDiplomaView.xaml
    /// </summary>
    public partial class EditDiplomaView : UserControl
    {
        int DiplomaUsersID;
        DataBaseController dbc = new DataBaseController();
        public EditDiplomaView(int userID)
        {
            DiplomaUsersID = userID;
            InitializeComponent();
            using (DataBase context = new DataBase())
            {
                var Diplomas = context.Diplomas.ToList();

                foreach (var diploma in Diplomas)
                {
                    if ("S1" == diploma.DiplomaName)
                    {
                        S1.Content = diploma.DiplomaName;
                        S1.Tag = diploma.DiplomaID;
                    }
                    if ("S2" == diploma.DiplomaName)
                    {
                        S2.Content = diploma.DiplomaName;
                        S2.Tag = diploma.DiplomaID;

                    }
                    if ("S3" == diploma.DiplomaName)
                    {
                        S3.Content = diploma.DiplomaName;
                        S3.Tag = diploma.DiplomaID;

                    }
                    if ("P1" == diploma.DiplomaName)
                    {
                        P1.Content = diploma.DiplomaName;
                        P1.Tag = diploma.DiplomaID;

                    }
                    if ("P2" == diploma.DiplomaName)
                    {
                        P2.Content = diploma.DiplomaName;
                        P2.Tag = diploma.DiplomaID;

                    }
                    if ("B1" == diploma.DiplomaName)
                    {
                        B1.Content = diploma.DiplomaName;
                        B1.Tag = diploma.DiplomaID;

                    }
                    if ("B2" == diploma.DiplomaName)
                    {
                        B2.Content = diploma.DiplomaName;
                        B2.Tag = diploma.DiplomaID;

                    }
                    if ("B3" == diploma.DiplomaName)
                    {
                        B3.Content = diploma.DiplomaName;
                        B3.Tag = diploma.DiplomaID;

                    }
                }

                var User_Diplomas = from x in context.User_Diplomas
                                  where x.UserID == userID && x.DeletedAt == null
                                  select x;

                foreach (var userRole in User_Diplomas)
                {
                    if (userRole.DiplomaID == int.Parse(S1.Tag.ToString()))
                    {
                        S1.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(S2.Tag.ToString()))
                    {
                        S2.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(S3.Tag.ToString()))
                    {
                        S3.IsChecked = true;
                    }
                    if (userRole.DiplomaID == int.Parse(B1.Tag.ToString()))
                    {
                        B1.IsChecked = true;
                    }
                    if (userRole.DiplomaID == int.Parse(B2.Tag.ToString()))
                    {
                        B2.IsChecked = true;
                    }
                    if (userRole.DiplomaID == int.Parse(B3.Tag.ToString()))
                    {
                        B3.IsChecked = true;
                    }
                    if (userRole.DiplomaID == int.Parse(P1.Tag.ToString()))
                    {
                        P1.IsChecked = true;
                    }
                    if (userRole.DiplomaID == int.Parse(P2.Tag.ToString()))
                    {
                        P2.IsChecked = true;
                    }
                }
            }
        }

        private void ButtonConfirm(object sender, RoutedEventArgs e)
        {
            using(DataBase context = new DataBase()) {
                foreach (CheckBox c in EditDiplomaLayout.Children.OfType<CheckBox>())
                {
                    if (c.IsChecked == true)
                    {
                        int diplomaID = int.Parse(c.Tag.ToString());
                        //int.Parse(c.Tag.ToString());


                        var User_Diplomas = context.User_Diplomas.Any(x => x.DiplomaID == diplomaID && x.DeletedAt == null && x.UserID == DiplomaUsersID);

                        if (User_Diplomas)
                        {

                        }
                        else
                        {
                            dbc.Add_UserDiploma(diplomaID, DiplomaUsersID);
                        }



                    }
                    else if (c.IsChecked == false)
                    {

                        int diplomaID = int.Parse(c.Tag.ToString());

                        var User_Diplomas = context.User_Diplomas.Any(x => x.DiplomaID == diplomaID && x.DeletedAt == null && x.UserID == DiplomaUsersID);

                        if (User_Diplomas)
                        {
                            dbc.Delete_UserDiploma(DiplomaUsersID, diplomaID);
                        }
                        else
                        {

                        }
                    }
                }
            }



            Switcher.Switch(new DiplomaList());
        }
                



        private void ButtonCancel(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DiplomaList());
        }
    }
}


