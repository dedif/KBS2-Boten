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
            this.Hide();
            MainWindow m = new MainWindow();
            m.Show();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(WeightBox.Text))
            {

                MessageBoxResult DataIncorrect = MessageBox.Show(
                    "U heeft niet alle gegevens ingevuld",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }


           else if (b.NameCheck(NameBox.Text) == false)
            {
                {
                    try
                    {
                        double Weight = double.Parse(WeightBox.Text);



                        int Rowers = int.Parse(RowersCombo.Text);
                        Boolean Steeringwheel = false;

                        if (SteeringWheelCheckbox.IsChecked == true)
                        {
                            Steeringwheel = true;
                        }

                        b.AddBoat(NameBox.Text, TypCombo.Text, Rowers, Weight, Steeringwheel);


                        MessageBoxResult Succes = MessageBox.Show(
                            "De boot is succesvol opgeslagen",
                            "Melding",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        switch (Succes)
                        {
                            case MessageBoxResult.OK:
                                this.Hide();
                                MainWindow m = new MainWindow();
                                m.Show();
                                break;

                        }
                    }
                    catch
                    {

                        MessageBoxResult WeightIncorrect = MessageBox.Show(
                            "Het gewicht moet een getal zijn",
                            "Melding",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }



                }
            }
            else
            {
                MessageBoxResult NameIncorrect = MessageBox.Show(
                       "Deze bootnaam bestaat al",
                       "Melding",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);

         
            }
        }
    }
}
