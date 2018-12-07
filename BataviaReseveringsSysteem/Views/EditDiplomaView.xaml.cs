using BataviaReseveringsSysteem.Database;
using Controllers;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for EditDiplomaView.xaml
    /// </summary>
    public partial class EditDiplomaView : UserControl
    {
        int DiplomaUsersID;
        DataBaseController dbc = new DataBaseController();
        public EditDiplomaView(int personID)
        {
            DiplomaUsersID = personID;
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

                var MemberDiplomas = from x in context.MemberDiplomas
                                     where x.PersonID == personID && x.Deleted_at == null
                                     select x;

                foreach (var memberRole in MemberDiplomas)
                {
                    if (memberRole.DiplomaID == int.Parse(S1.Tag.ToString()))
                    {
                        S1.IsChecked = true;
                    }

                    if (memberRole.DiplomaID == int.Parse(S2.Tag.ToString()))
                    {
                        S2.IsChecked = true;
                    }

                    if (memberRole.DiplomaID == int.Parse(S3.Tag.ToString()))
                    {
                        S3.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(B1.Tag.ToString()))
                    {
                        B1.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(B2.Tag.ToString()))
                    {
                        B2.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(B3.Tag.ToString()))
                    {
                        B3.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(P1.Tag.ToString()))
                    {
                        P1.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(P2.Tag.ToString()))
                    {
                        P2.IsChecked = true;
                    }
                }
            }
        }

        private void ButtonConfirm(object sender, RoutedEventArgs e)
        {
            using (DataBase context = new DataBase())
            {
                foreach (CheckBox c in EditDiplomaLayout.Children.OfType<CheckBox>())
                {
                    if (c.IsChecked == true)
                    {
                        int diplomaID = int.Parse(c.Tag.ToString());
                        //int.Parse(c.Tag.ToString());


                        var MemberDiplomas = context.MemberDiplomas.Any(x => x.DiplomaID == diplomaID && x.Deleted_at == null && x.PersonID == DiplomaUsersID);

                        if (MemberDiplomas)
                        {

                        }
                        else
                        {
                            dbc.Add_MemberDiploma(diplomaID, DiplomaUsersID);
                        }



                    }
                    else if (c.IsChecked == false)
                    {

                        int diplomaID = int.Parse(c.Tag.ToString());

                        var MemberDiplomas = context.MemberDiplomas.Any(x => x.DiplomaID == diplomaID && x.Deleted_at == null && x.PersonID == DiplomaUsersID);

                        if (MemberDiplomas)
                        {
                            dbc.Delete_MemberDiploma(DiplomaUsersID, diplomaID);
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