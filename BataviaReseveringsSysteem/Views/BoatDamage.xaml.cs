using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Controllers;

namespace Views
{
    /// <summary>
    /// Interaction logic for BoatDamage.xaml
    /// </summary>
    public partial class BoatDamage : UserControl
    {
        DataBase context = new DataBase();
        DamageController damageController = new DamageController();

        public string Reserved { get; set; } = null;
        public BoatDamage()
        {

            InitializeComponent();

            var NameBoats = (from data in context.Boats
                             where data.DeletedAt == null
                             select data.Name).ToList();

            if (NameBoats.Count < 1)
            {
                //Als er geen boten zijn dan laat de applicatie dat zien
                NameboatCombo.IsEnabled = false;
                DescriptionBox.IsEnabled = false;
                HeavyDamageRadioButton.IsEnabled = false;
                LightDamageRadioButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                MessageLabel.Content = "Er zijn geen boten beschikbaar.";
                MessageLabel.Visibility = Visibility.Visible;
                DamagesLabel.Visibility = Visibility.Hidden;
                OtherDamages.Visibility = Visibility.Hidden;
            }

            foreach (string name in NameBoats)
            {
                NameboatCombo.Items.Add(name);
            }
            NameboatCombo.SelectedIndex = 0;

            LightDamageRadioButton.IsChecked = true;

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
                            Reserved,
                            "Bevestigen",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                switch (Melding)
                {

                    case MessageBoxResult.Yes:

                        var Boat = (from data in context.Boats
                                    where data.Name == NameboatCombo.Text && data.DeletedAt == null
                                    select data).Single();

                        var Reservations = (from data in context.Reservations
                                            join boats in context.Boats on data.BoatID equals boats.BoatID
                                            where boats.Name.Equals(NameboatCombo.Text)
                                            where data.Deleted == null
                                            select data).ToList();


                        string status = null;
                        if (LightDamageRadioButton.IsChecked == true)
                        {
                            status = "Lichte schade";
                            Boat.Broken = false;
                        }
                        else if (HeavyDamageRadioButton.IsChecked == true)
                        {
                            status = "Zware schade";
                            Boat.Broken = true;
                            //De methode om mails te sturen, wordt aangeroepen
                            damageController.SendingMail(Reservations);
                            //voor elke reservering met zware schade wordt deleted vadaag
                            foreach (Reservation r in Reservations)
                            {
                                r.Deleted = DateTime.Now;
                            }
                        }

                        Damage damage = new Damage(LoginView.UserId, Boat.BoatID, DescriptionBox.Text, status);

                        context.Damages.Add(damage);
                        context.SaveChanges();
                        Switcher.Switch(new Dashboard());
                        break;

                    case MessageBoxResult.No:
                        Reserved = "";
                        break;
                }
            }
        }


        private void HeavyDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Visible;
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
            var Reservation = (from data in context.Reservations
                               join boats in context.Boats on data.BoatID equals boats.BoatID
                               select boats.Name).ToList();
            if (Reservation.Contains(boat))
            {
                Reserved = " Deze boot is in de toekomst gereserveerd.";
            }
        }


        private void NameboatCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Aan het begin wordt de textblock Otherdamages leeg gemaakt.	
            OtherDamages.Text = "";
            using (DataBase context = new DataBase())
            {
                //Dit selecteerd alle beschrijvingen van de schade's van de geselecteerde boot	
                var SelectedBoat = (
                    from data in context.Damages
                    join boats in context.Boats
                    on data.BoatID equals boats.BoatID
                    where boats.Name == (string)NameboatCombo.SelectedValue
                    where data.Status != "Hersteld"
                    select data.Description).ToList();

                foreach (string description in SelectedBoat)
                {
                    //De schade van de geselecteerde boot worden in het textblock OtherDamages gezet	
                    OtherDamages.Text += "\n" + description + "\n";
                }
                if (SelectedBoat.Count < 1)
                {
                    //Als er een schade's zijn voor de geselecteerde boot word de Label en textblock niet getoond op het scherm	
                    DamagesLabel.Visibility = Visibility.Hidden;
                    OtherDamages.Visibility = Visibility.Hidden;
                }
                else
                {
                    DamagesLabel.Visibility = Visibility.Visible;
                    OtherDamages.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
