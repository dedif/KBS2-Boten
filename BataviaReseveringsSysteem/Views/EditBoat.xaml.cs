using BataviaReseveringsSysteem.Database;
using Controllers;
using ScreenSwitcher;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{

    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditBoat : UserControl
    {

        private BoatController b = new BoatController();
        private int EditBoatID;
        public EditBoat(int id)
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            EditBoatID = id;

            using (DataBase context = new DataBase())
            {
                var boats = from x in context.Boats
                            where x.BoatID == id
                            select x;

                foreach (var boat in boats)
                {
                    if (TypCombo.SelectedItem == skiffItem)
                    {
                        RowersCombo.IsEnabled = false;
                        boat.NumberOfRowers = 1;
                    }
                    else if (TypCombo.SelectedItem != skiffItem)
                    {
                        RowersCombo.IsEnabled = true;
                        oneRower.Visibility = Visibility.Hidden;
                        RowersCombo.SelectedItem = boat.NumberOfRowers;
                    }

                    NameBox.Text = boat.Name;
                    RowersCombo.Text = boat.NumberOfRowers.ToString();
                    WeightBox.Text = boat.Weight.ToString();
                    TypCombo.SelectedItem = boat.Type;
                    TypCombo.Text = boat.Type.ToString();
                    BoatLocationBox.Text = boat.BoatLocation.ToString();
                    AvailableAt.SelectedDate = boat.AvailableAt;
                    if (boat.Steering == true)
                    {
                        SteeringWheelToggle.IsChecked = true;
                    }
                }
            }
        }

        public void EditBoatDiploma()
        {
            using (DataBase context = new DataBase())
            {
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
                                   where x.BoatID == EditBoatID
                                   select x;

                foreach (var memberRole in BoatDiplomas)
                {
                    if (memberRole.DiplomaID == int.Parse(S1CheckBox.Tag.ToString()))
                    {
                        S1CheckBox.IsChecked = true;
                    }

                    if (memberRole.DiplomaID == int.Parse(S2CheckBox.Tag.ToString()))
                    {
                        S2CheckBox.IsChecked = true;
                    }

                    if (memberRole.DiplomaID == int.Parse(S3CheckBox.Tag.ToString()))
                    {
                        S3CheckBox.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(B1CheckBox.Tag.ToString()))
                    {
                        B1CheckBox.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(B2CheckBox.Tag.ToString()))
                    {
                        B2CheckBox.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(B3CheckBox.Tag.ToString()))
                    {
                        B3CheckBox.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(P1CheckBox.Tag.ToString()))
                    {
                        P1CheckBox.IsChecked = true;
                    }
                    if (memberRole.DiplomaID == int.Parse(P2CheckBox.Tag.ToString()))
                    {
                        P2CheckBox.IsChecked = true;
                    }
                }
            }

        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (b.WhiteCheck(NameBox.Text, WeightBox.Text, BoatLocationBox.Text) == true)
            {
                using (DataBase context = new DataBase())
                {


                    // bool boatLocationCheck = context.Boats.Where(y => y.BoatID != EditBoatID).Any(x => x.BoatLocation == BoatLocation && x.DeletedAt == null);

                    if (b.WeightCheck(WeightBox.Text) == true)
                    {
                        if (b.LocationCheckIfInt(BoatLocationBox.Text))
                        {
                            int BoatLocation = int.Parse(BoatLocationBox.Text);


                            if (b.EditBoatLocationCheck(BoatLocation, EditBoatID) == true)
                            {
                                if (b.EditBoatNameCheck(NameBox.Text, EditBoatID) == true)
                                {

                                    double Weight = double.Parse(WeightBox.Text);
                                    int Rowers = int.Parse(RowersCombo.Text);
                                    Boolean Steeringwheel = false;

                                    if (SteeringWheelToggle.IsChecked == true)
                                    {
                                        Steeringwheel = true;
                                    }


                                    // if (!boatLocationCheck)
                                    // {

                                    b.UpdateBoat(EditBoatID, NameBox.Text, TypCombo.Text, Rowers, Weight, Steeringwheel, BoatLocation, AvailableAt.SelectedDate.Value);
                                    MessageBoxResult Succes = MessageBox.Show(
                                     "De boot is succesvol opgeslagen",
                                     "Melding",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Information);


                                    switch (Succes)
                                    {
                                        case MessageBoxResult.OK:
                                            Switcher.Switch(new BoatList());
                                            break;
                                    }
                                }
                            }
                        }
                        NotificationLabel.Content = b.Notification();
                    }
                }
            }
            NotificationLabel.Content = b.Notification();
            EditBoatDiploma();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());
        }

        private void TypCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypCombo.SelectedIndex == 1)
            {
                RowersCombo.SelectedIndex = 0;
                RowersCombo.IsEnabled = false;
                SteeringWheelToggle.IsChecked = false;
                SteeringWheelToggle.IsEnabled = false;
            }
            else
            {
                RowersCombo.IsEnabled = true;
                SteeringWheelToggle.IsChecked = true;
                SteeringWheelToggle.IsEnabled = true;
            }
        }
    }
}




