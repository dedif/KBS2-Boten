using BataviaReseveringsSysteem;
using BataviaReseveringsSysteem.Database;
using Controllers;
using ScreenSwitcher;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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


                using(DataBase context = new DataBase())
                {
                    var boats = from x in context.Boats
                                where x.BoatID == id
                                select x;

                    foreach (var boat in boats)
                    {
                        NameBox.Text = boat.Name;
                        NameBox.IsEnabled = false;
                        RowersCombo.SelectedItem = boat.NumberOfRowers;
                        RowersCombo.Text = boat.NumberOfRowers.ToString();
                        WeightBox.Text = boat.Weight.ToString();
                        TypCombo.SelectedItem = boat.Type;
                        TypCombo.Text = boat.Type.ToString();
                        if(boat.Steering == true)
                        {
                            SteeringWheelCheckbox.IsChecked = true;
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
            if (b.WhiteCheck(NameBox.Text, WeightBox.Text) == true)
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

                        b.UpdateBoat(EditBoatID, NameBox.Text, TypCombo.Text, Rowers, Weight, Steeringwheel);

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
                SteeringWheelCheckbox.IsChecked = false;
                SteeringWheelCheckbox.IsEnabled = false;
            }
            else
            {

                RowersCombo.IsEnabled = true;
                SteeringWheelCheckbox.IsChecked = true;
                SteeringWheelCheckbox.IsEnabled = true;
            }

        }
    }

    }



