﻿using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for BoatDamage.xaml
    /// </summary>
    public partial class BoatDamage : UserControl
    {
        DataBase context = new DataBase();

        string reserved = null;
        public BoatDamage()
        {
            InitializeComponent();

            var NameBoats = (from data in context.Boats
                            select data.Name).ToList();

            foreach (string name in NameBoats) {
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
            if (!IsEmpty(DescriptionBox.Text)) { 
                AlreadyReserved(NameboatCombo.Text);
                MessageBoxResult Melding = MessageBox.Show(
                            "Weet u zeker dat u deze schade wilt melden?" +
                            reserved,
                            "Melding",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                    switch (Melding)
                    {
                        case MessageBoxResult.Yes:

                            var Boat = (from data in context.Boats
                                        where data.Name == NameboatCombo.Text
                                        select data).Single();

                            var Reservations = (from data in context.Reservations
                                               where data.Boat.Name == NameboatCombo.Text
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
                            foreach (Reservation r in Reservations)
                            {
                            r.Deleted = DateTime.Now;
                            }
                            }

                            Damage damage = new Damage(LoginView.LoggedUser.PersonID, Boat.BoatID, DescriptionBox.Text, status);
                            
                            context.Damages.Add(damage);
                            context.SaveChanges();

                            Switcher.Switch(new Dashboard());
                            break;

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
                        select data.Boat.Name).ToList();
            if (Reservation.Contains(boat))
            {
                reserved = " Deze boot is in de toekomst gereserveerd.";
            }
        }
    }
}