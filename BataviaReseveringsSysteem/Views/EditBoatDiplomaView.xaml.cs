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
    /// Interaction logic for EditBoatDiplomaView.xaml
    /// </summary>
    public partial class EditBoatDiplomaView : UserControl
    {
      private  int DiplomaBoatID;
      private BoatController bc = new BoatController();
        public EditBoatDiplomaView(int boatID)
        {
            InitializeComponent();
            DiplomaBoatID = boatID;
            InitializeComponent();
            using (DataBase context = new DataBase())
            {
                var Boat = (from data in context.Boats
                            where data.BoatID == boatID
                            select data).Single();

                Name.Content = Boat.Name;
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

                var BoatDiplomas = from x in context.Boat_Diplomas
                                     where x.BoatID == boatID 
                                     select x;

                foreach (var BoatDiploma in BoatDiplomas)
                {
                    if (BoatDiploma.DiplomaID == int.Parse(S1CheckBox.Tag.ToString()))
                    {
                        S1CheckBox.IsChecked = true;
                    }

                    if (BoatDiploma.DiplomaID == int.Parse(S2CheckBox.Tag.ToString()))
                    {
                        S2CheckBox.IsChecked = true;
                    }

                    if (BoatDiploma.DiplomaID == int.Parse(S3CheckBox.Tag.ToString()))
                    {
                        S3CheckBox.IsChecked = true;
                    }
                    if (BoatDiploma.DiplomaID == int.Parse(B1CheckBox.Tag.ToString()))
                    {
                        B1CheckBox.IsChecked = true;
                    }
                    if (BoatDiploma.DiplomaID == int.Parse(B2CheckBox.Tag.ToString()))
                    {
                        B2CheckBox.IsChecked = true;
                    }
                    if (BoatDiploma.DiplomaID == int.Parse(B3CheckBox.Tag.ToString()))
                    {
                        B3CheckBox.IsChecked = true;
                    }
                    if (BoatDiploma.DiplomaID == int.Parse(P1CheckBox.Tag.ToString()))
                    {
                        P1CheckBox.IsChecked = true;
                    }
                    if (BoatDiploma.DiplomaID == int.Parse(P2CheckBox.Tag.ToString()))
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
                List<CheckBox> CheckboxList = new List<CheckBox>() { S1CheckBox, S2CheckBox, S3CheckBox, B1CheckBox, B2CheckBox, B3CheckBox, P1CheckBox, P2CheckBox };

                foreach (CheckBox c in CheckboxList)
                {
                    if (c.IsChecked == true)
                    {
                        int diplomaID = int.Parse(c.Tag.ToString());
                        //int.Parse(c.Tag.ToString());


                        var BoatDiplomas = context.Boat_Diplomas.Any(x => x.DiplomaID == diplomaID && x.BoatID == DiplomaBoatID);

                        if (BoatDiplomas)
                        {

                        }
                        else
                        {
                            bc.Add_BoatDiploma(diplomaID, DiplomaBoatID);
                        }



                    }
                    else if (c.IsChecked == false)
                    {

                        int diplomaID = int.Parse(c.Tag.ToString());

                        var MemberDiplomas = context.Boat_Diplomas.Any(x => x.DiplomaID == diplomaID &&  x.BoatID == DiplomaBoatID);

                        if (MemberDiplomas)
                        {
                            bc.Delete_BoatDiploma(DiplomaBoatID, diplomaID);
                        }
                       
                    }
                }
            }



            Switcher.Switch(new BoatDiplomaList());
        }




        private void ButtonCancel(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDiplomaList());
        }
    }
}
