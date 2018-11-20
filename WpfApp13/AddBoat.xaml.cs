using ConsoleApp1;
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
using System.Windows.Shapes;

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddBoat : Window
    {
        Boatcontroller b = new Boatcontroller();

        public AddBoat()
        {
            InitializeComponent();
           

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
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
                               this.Close();
                                break;

                        }
                    }

                }
                //idiallle gewicht 70,  gewicht klasse ipv gewicht

            }
            
            NotificationLabel.Content = b.Notification();
        }

    }
}

