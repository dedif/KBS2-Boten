using WpfApp13;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddBoat : UserControl
    {
        Boatcontroller b = new Boatcontroller();

        public AddBoat()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainWindow());
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

                        if (SteeringWheelCheckbox.IsChecked == true)
                        {
                            Steeringwheel = true;
                        }

                        b.AddBoat(NameBox.Text, TypCombo.Text, Rowers, Weight, Steeringwheel);

                        NotificationLabel.Content = b.Notification();

                        MessageBoxResult Succes = MessageBox.Show(
                            "De boot is succesvol opgeslagen",
                            "Melding",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        switch (Succes)
                        {
                            case MessageBoxResult.OK:
                                Switcher.Switch(new MainWindow());
                                break;

                        }
                    }

                }
            }
            
            NotificationLabel.Content = b.Notification();
        }

    }
}

