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

                                    System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("De boot is succesvol opgeslagen", "Succes", System.Windows.Forms.MessageBoxButtons.OK, 30000);

                                    switch (Succes)
                                    {
                                        case System.Windows.Forms.DialogResult.OK:
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




