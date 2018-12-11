using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem.Views;
using Controllers;
using Models;
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
    /// Interaction logic for EditBoatDamage.xaml
    /// </summary>
    public partial class EditBoatDamage : UserControl
    {
        private string reserved = null;
        private int BoatID;
        private BoatController bc = new BoatController();
        public EditBoatDamage(int boatID)
        {
            InitializeComponent();
            using (DataBase context = new DataBase())
            {
                var NameBoats = (from data in context.Boats
                                 select data.Name).ToList();

                foreach (string name in NameBoats)
                {
                    NameboatCombo.Items.Add(name);
                }
                NameboatCombo.SelectedIndex = 0;

                LightDamageRadioButton.IsChecked = true;

                var BoatDamages = (from data in context.Damages where data.BoatID == boatID
                                  select data).ToList();

                foreach(var BoatDamage in BoatDamages)
                {
                    if(BoatDamage.Status == "Zware schade")
                    {
                        HeavyDamageRadioButton.IsChecked = true;
                    } else if (BoatDamage.Status == "Lichte schade")
                    {
                        LightDamageRadioButton.IsChecked = true;

                    } else if(BoatDamage.Status == "Geen schade")
                    {
                        NoDamageRadioButton.IsChecked = true;
                    }
                    DescriptionBox.Text = BoatDamage.Description;
                    
                }
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //kijkt of beschrijving is ingevuld
            if (!IsEmpty(DescriptionBox.Text))
            {
                //kijkt of reservering al is gereserveerd
                AlreadyReserved(NameboatCombo.Text);
                MessageBoxResult Melding = MessageBox.Show(
                            "Weet u zeker dat u deze schade wilt melden?" +
                            // "Boot is gereserveerd in de toekomst" als boot is gereserveerd. Anders null
                            reserved,
                            "Melding",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                switch (Melding)
                {
                    case MessageBoxResult.Yes:
                        using (DataBase context = new DataBase()) {
                            var Boat = (from data in context.Boats
                                        where data.Name == NameboatCombo.Text && data.DeletedAt == null
                                        select data).Single();

                            var Reservations = (from data in context.Reservations
                                                where data.Boat.Name == NameboatCombo.Text
                                                where data.Deleted == null
                                                select data).ToList();


                            string status = null;
                            if (LightDamageRadioButton.IsChecked == true)
                            {
                                status = "Lichte schade";
                            }
                            else if (HeavyDamageRadioButton.IsChecked == true)
                            {
                                status = "Zware schade";
                                Boat.Broken = true;
                                //voor elke reservering met zware schade wordt deleted vadaag
                                foreach (Reservation r in Reservations)
                                {
                                    r.Deleted = DateTime.Now;
                                } 
                            } else if(NoDamageRadioButton.IsChecked == true)
                                {
                                status = "Geen schade";
                                
                            }

                            bc.UpdateBoatDamage(Boat.BoatID, DescriptionBox.Text, status);

                           

                            Switcher.Switch(new BoatDamageList());
                            break;
                        }

                    case MessageBoxResult.No:
                        reserved = "";
                        break;
                
                }
            }
        }

        private void HeavyDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Visible;
        }

        private void NoDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Hidden;
        }

        private void LightDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Hidden;
        }
        //deze methode kijkt of je de beschrijving hebt ingevuld. de boot kan nooit leeg zijn.
        public bool IsEmpty(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageLabel.Visibility = Visibility.Visible;
                return true;
            }
            else
            {
                return false;
            }
        }
        //kijkt of de boot gereserveerd is
        public void AlreadyReserved(string boat)
        {
            using (DataBase context = new DataBase())
            {
                var Reservation = (from data in context.Reservations
                                   select data.Boat.Name).ToList();
                if (Reservation.Contains(boat))
                {
                    reserved = " Deze boot is in de toekomst gereserveerd.";
                }
            }
        }
    }
}
