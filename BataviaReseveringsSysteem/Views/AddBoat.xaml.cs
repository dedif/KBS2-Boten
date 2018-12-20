using Controllers;
using ScreenSwitcher;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft;
using BataviaReseveringsSysteem.Controllers;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddBoat : UserControl
    {
        BoatController b = new BoatController();

        public AddBoat()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            TypCombo.SelectedIndex = 1;
            RowersCombo.SelectedItem = "1";
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }
        //Deze methode checkt op whitespace in de textvelden, de uniekheid van de Naam die is ingevoerd en dat gewicht juist is ingevoerd
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (b.WhiteCheck(NameBox.Text, WeightBox.Text, BoatLocationBox.Text) == true)
            {
                if (b.NameCheck(NameBox.Text) == true)
                {
                    if (b.WeightCheck(WeightBox.Text) == true)
                    {
                        if (b.LocationCheckIfInt(BoatLocationBox.Text))
                        {
                            int BoatLocation = int.Parse(BoatLocationBox.Text);
                            if (b.BoatLocationCheck(BoatLocation) == true)
                            {
                                double Weight = double.Parse(WeightBox.Text);
                                int Rowers = int.Parse(RowersCombo.Text);
                                Boolean Steeringwheel = false;

                                if (SteeringWheelToggle.IsChecked == true)
                                {
                                    Steeringwheel = true;
                                }
                                List<CheckBox> listDiplomaCheckBox = new List<CheckBox> { S1CheckBox, S2CheckBox, S3CheckBox, B1CheckBox, B2CheckBox, B3CheckBox, P1CheckBox, P2CheckBox };

                                //De methode AddBoat wordt aangeroepen om een nieuwe boot toe te voegen aan de database
                              


                                //Als de boot succesvol is toegevoegd aan de database, laat de applicatie een pop-up scherm zien. 
                                NotificationLabel.Content = b.Notification();


                                System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("De boot is succesvol opgeslagen", "Melding", System.Windows.Forms.MessageBoxButtons.YesNo, 30000);


                                //System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show(
                                //    "De boot is succesvol opgeslagen",
                                //    "Melding",
                                //    MessageBoxButton.OK, 
                                //    MessageBoxImage.Information,85);

                                switch (Succes)
                                {
                                    case System.Windows.Forms.DialogResult.None:

                                        break;
                                    case System.Windows.Forms.DialogResult.Yes:
                                        b.AddBoat(NameBox.Text, TypCombo.Text, Rowers, Weight, Steeringwheel, BoatLocation, AvailableAt.SelectedDate.Value);
                                        b.AddDiploma(listDiplomaCheckBox);
                                        Switcher.Switch(new BoatList());
                                        break;

                                }


                              
                            }
                        }
                    }

                }
            }
            NotificationLabel.Content = b.Notification();
        }

       

        private void TypCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Als Skiff is gekozen dan is het aantal roeiers 1 en de de toggle is uit. 
            if (TypCombo.SelectedIndex == 1)
            {
                RowersCombo.Items.Insert(0, "1");
                RowersCombo.SelectedIndex = 0;
                RowersCombo.IsEnabled = false;
                SteeringWheelToggle.IsChecked = false;
                SteeringWheelToggle.IsEnabled = false;
                SkiffLabel.Visibility = Visibility.Visible;
            }
            else
            {
                RowersCombo.Items.Remove("1");
                RowersCombo.SelectedIndex = 0;
                RowersCombo.IsEnabled = true;
                SteeringWheelToggle.IsEnabled = true;
                SkiffLabel.Visibility = Visibility.Hidden;
            }
        }
    }
}

