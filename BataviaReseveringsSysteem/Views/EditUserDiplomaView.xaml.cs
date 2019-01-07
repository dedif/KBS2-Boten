using BataviaReseveringsSysteem.Database;
using Controllers;
using ScreenSwitcher;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for EditDiplomaView.xaml
    /// </summary>
    public partial class EditUserDiplomaView : UserControl
    {
        int DiplomaUsersID;
        DataBaseController dbc = new DataBaseController();

        public EditUserDiplomaView(int userID)

        {
            DiplomaUsersID = userID;
            InitializeComponent();
            
            using (DataBase context = new DataBase())
            {
                var User = (from data in context.Users
                            where data.UserID == userID
                            select data).Single();
                
                Name.Content = $"{User.Firstname} {User.Middlename} {User.Lastname}";
                var Diplomas = context.Diplomas.ToList();

                foreach (var diploma in Diplomas)
                {
                    if ("S1" == diploma.DiplomaName)
                    {
                        S1CheckBox.Content = diploma.DiplomaName;
                        S1CheckBox.Tag = diploma.DiplomaID;
                    }
                    if ("S2" == diploma.DiplomaName)
                    {
                        S2CheckBox.Content = diploma.DiplomaName;
                        S2CheckBox.Tag = diploma.DiplomaID;

                    }
                    if ("S3" == diploma.DiplomaName)
                    {
                        S3CheckBox.Content = diploma.DiplomaName;
                        S3CheckBox.Tag = diploma.DiplomaID;

                    }
                    if ("P1" == diploma.DiplomaName)
                    {
                        P1CheckBox.Content = diploma.DiplomaName;
                        P1CheckBox.Tag = diploma.DiplomaID;

                    }
                    if ("P2" == diploma.DiplomaName)
                    {
                        P2CheckBox.Content = diploma.DiplomaName;
                        P2CheckBox.Tag = diploma.DiplomaID;

                    }
                    if ("B1" == diploma.DiplomaName)
                    {
                        B1CheckBox.Content = diploma.DiplomaName;
                        B1CheckBox.Tag = diploma.DiplomaID;

                    }
                    if ("B2" == diploma.DiplomaName)
                    {
                        B2CheckBox.Content = diploma.DiplomaName;
                        B2CheckBox.Tag = diploma.DiplomaID;

                    }
                    if ("B3" == diploma.DiplomaName)
                    {
                        B3CheckBox.Content = diploma.DiplomaName;
                        B3CheckBox.Tag = diploma.DiplomaID;

                    }
                }


                var User_Diplomas = from x in context.User_Diplomas
                                  where x.UserID == userID && x.DeletedAt == null
                                  select x;
    

                foreach (var userRole in User_Diplomas)
                {

                    if (userRole.DiplomaID == int.Parse(S1CheckBox.Tag.ToString()))
                    {
                        S1CheckBox.IsChecked = true;
                    }


                    if (userRole.DiplomaID == int.Parse(S2CheckBox.Tag.ToString()))
                    {
                        S2CheckBox.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(S3CheckBox.Tag.ToString()))

                    {
                        S3CheckBox.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(B1CheckBox.Tag.ToString()))
                    {
                        B1CheckBox.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(B2CheckBox.Tag.ToString()))
                    {
                        B2CheckBox.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(B3CheckBox.Tag.ToString()))
                    {
                        B3CheckBox.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(P1CheckBox.Tag.ToString()))
                    {
                        P1CheckBox.IsChecked = true;
                    }

                    if (userRole.DiplomaID == int.Parse(P2CheckBox.Tag.ToString()))
                    {
                        P2CheckBox.IsChecked = true;
                    }
                }
            }
        }

        private void ButtonConfirm(object sender, RoutedEventArgs e)
        {
            using (DataBase context = new DataBase())
            {
                List<CheckBox> CheckBoxList = new List<CheckBox>() { S1CheckBox, S2CheckBox, S3CheckBox, B1CheckBox, B2CheckBox, B3CheckBox, P1CheckBox, P2CheckBox };
                foreach (CheckBox c in CheckBoxList)
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

            System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("De diploma's van dit lid zijn aangepast", "Bevestiging diploma's", System.Windows.Forms.MessageBoxButtons.OK, 30000);

            switch (Succes)
            {

                case System.Windows.Forms.DialogResult.OK:


                    Switcher.Switch(new UserDiplomaList());
                    break;

            }

        }




        private void ButtonCancel(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserDiplomaList());
        }
    }
}