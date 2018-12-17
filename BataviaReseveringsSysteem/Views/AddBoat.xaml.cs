using Controllers;
using ScreenSwitcher;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;


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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());
        }
        //Deze methode checkt op whitespace in de textvelden, de uniekheid van de Naam die is ingevoerd en dat gewicht juist is ingevoerd
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (b.WhiteCheck(NameBox.Text, WeightBox.Text) == true)
            {

                if (b.NameCheck(NameBox.Text) == true)
                {
                    if (b.WeightCheck(WeightBox.Text) == true)
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
                        b.AddBoat(NameBox.Text, TypCombo.Text, Rowers, Weight, Steeringwheel);
                        b.AddDiploma(listDiplomaCheckBox);


                        //Als de boot succesvol is toegevoegd aan de database, laat de applicatie een pop-up scherm zien. 
                        NotificationLabel.Content = b.Notification();

                        MessageBoxResult Succes = MessageBox.Show(
                            "De boot is succesvol opgeslagen",
                            "Melding",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        switch (Succes)
                        {
                            case MessageBoxResult.OK:
                                Switcher.Switch(new Dashboard());
                                break;

                        }
                    }

                }
            }
            NotificationLabel.Content = b.Notification();
        }


        private void TypCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (TypCombo.SelectedIndex == 1)
                {
                RowersCombo.SelectedIndex = 0;
                RowersCombo.IsEnabled = false;
                SteeringWheelToggle.IsChecked = false;
                SteeringWheelToggle.IsEnabled = false;
                SkiffLabel.Visibility = Visibility.Visible;
                }
            else 
            {
           
                RowersCombo.IsEnabled = true;
                SteeringWheelToggle.IsEnabled = true;
                SkiffLabel.Visibility = Visibility.Hidden;
            }
        }
    }
}

