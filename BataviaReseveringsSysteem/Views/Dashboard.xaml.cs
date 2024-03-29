﻿using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem.Views;
using ScreenSwitcher;
using Controllers;
using BataviaReseveringsSysteem.Controllers;
using System;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard
    {
        public int YLeft = 10;
        public int YRight = 10;
        public int Count;
        public int MaxReservationUser = 2;
        //Deze lijsten, bevatten alle buttens en labels
        public List<Label> LabelList = new List<Label>();
        public List<Button> ButtonList = new List<Button>();
        DataBase context = new DataBase();
        DashboardController dashboardController;
        public static NavigationView navigationview;
        bool competition = false;
        bool coach = false;
        public Dashboard()
        {
            InitializeComponent();
            
            UserTimeOutController utoc = new UserTimeOutController(System.Windows.Input.FocusManager.GetFocusedElement(this), 90);


            var loggedUser = (from data in context.Users
                              where data.UserID == LoginView.UserId
                              select data).Single();


            dashboardController = new DashboardController(this);
           
            var rol = (from data in context.User_Roles
                       where data.UserID == LoginView.UserId
                       select data.RoleID).ToList();

            //Een wedstrijd commisaris heeft maximaal 8 afschrijvingen. 
            //De wedstrijdcommisaris heeft ook de keuze tussen afschrijvingen voor een wedstrijd en persoonlijke afschrijvingen.
            if (rol.Contains(3))
            {
                MaxReservationUser = 8;
                //SortReservation.Visibility = Visibility.Visible;
                SortReservationLabel.Visibility = Visibility.Visible;

            }
            

           
            if (!rol.Contains(2))
            {
                SelectReservation.Items.Remove((ComboBoxItem)Coach);
            }
            if (!rol.Contains(3))
            {
                SelectReservation.Items.Remove((ComboBoxItem)Competition);

            }

            //Een Coach heeft maximaal 8 afschrijvingen.
            if (rol.Contains(2))
            {
                MaxReservationUser = 8;
            }

            //Een examinator en bestuur mag zoveel afschrijvingen als die wilt
            if (rol.Contains(4) || rol.Contains(5))
            {
                MaxReservationUser = int.MaxValue;
            }

            //De reservaties van de gebruiker worden met deze methode getoond op het scherm
            ShowReservations(competition, coach);
            dashboardController.Notification(loggedUser.LastLoggedIn);

            // combobox index start op normale reservering
            int selectedIndex = 0;
            SelectReservation.SelectedItem = SelectReservation.Items.GetItemAt(selectedIndex);

            // haal de nieuwsberichten op
            var getNewsMessage = (from data in context.News_Messages
                                  where data.DeletedAt == null
                                  select data).ToList();

            NewsMessageBox.ItemsSource = getNewsMessage;

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ShowNewsMessage((int)(sender as TextBox).Tag));
        }



        public void ShowReservations(bool competition, bool coach)
        {
            using (var context = new DataBase())
            {

                //Geeft de reserveringen van de gebruiker
                var reservations = (
                    from data in context.Reservations
                    where data.Deleted == null
                    where data.UserId == LoginView.UserId
                    where data.Competition == competition
                    where data.Coach == coach
                    select data).ToList();

                //Dit geeft het totale aantal reserveren van de gebruiker
                var TotalReservations =(
                    from data in context.Reservations
                    where data.Deleted == null
                    where data.UserId == LoginView.UserId
                    select data).ToList();


                //Als de gebruiker nog geen afschrijvingen heeft, dan komt dit op het scherm te staan. 

                if (!reservations.Any())

                {
                    NoReservationLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    NoReservationLabel.Visibility = Visibility.Hidden;
                }

                //Als de gebruiker het maximale aantal afschrijvingen heeft bereikt, mag hij geen boten meer afschrijven
                if (TotalReservations.Count >= MaxReservationUser)

                {
                    MaxReservations.Visibility = Visibility.Visible;
                    navigationview.MakeAddReservationInvisible(false);
                }
                else
                {
                    MaxReservations.Visibility = Visibility.Hidden;
                    navigationview.MakeAddReservationInvisible(true);
                }
                foreach (var r in reservations)

                {
                    if (Count % 2 == 0)
                    {

                        //Dit is voor de label aan de linkerkant van de twee rijen
                        var l = new Label
                        {
                            Content = dashboardController.ReservationContent(r),
                            Margin = new Thickness(20, YLeft, 0, 0),
                            Width = 235,
                            FontSize = 16,


                        };
                        LabelList.Add(l);
                        var deleteButton = dashboardController.AddDeleteButton(25, YLeft + 130, r.ReservationID);
                        ButtonList.Add(deleteButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        reservationsCanvas.Children.Add(l);
                        reservationsCanvas.Children.Add(deleteButton);

                        YLeft = YLeft + 200;
                    }
                    else if (Count % 2 != 0)
                    {
                        //Hiermee maak je een label
                        var l2 = new Label
                        {
                            Content = dashboardController.ReservationContent(r),
                            Margin = new Thickness(355, YRight, 0, 0),
                            Width = 235,
                            FontSize = 16,
                        };
                        LabelList.Add(l2);
                        var deleteButton = dashboardController.AddDeleteButton(360, YRight + 130, r.ReservationID);
                        ButtonList.Add(deleteButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        reservationsCanvas.Children.Add(l2);
                        reservationsCanvas.Children.Add(deleteButton);


                        YRight = YRight + 200;
                    }


                    Count++;
                }
                reservationsCanvas.Height = YLeft + 10;

            }
        }

        //Deze methode verwijderd alle controls
        public void DeleteAllControls()
        {
            foreach (var t in LabelList)
            {
                reservationsCanvas.Children.Remove(t);
            }

            foreach (var t in ButtonList)
            {
                reservationsCanvas.Children.Remove(t);
            }
            // de posities worden gereset
            YLeft = 10;
            YRight = 10;
            Count = 0;
        }

    public void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var b = (Button)sender;
        dashboardController.DeleteReservation((int)b.Tag, competition, coach);
    }

        public void Change_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e) =>
            Switcher.Switch(new BoatSelectionView());

        private void SelectReservationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int SelectedValue = int.Parse(((ComboBoxItem)SelectReservation.SelectedItem).Tag.ToString());
            if (SelectedValue == 2)
            {
                competition = false;
                coach = true;
                SortReservationLabel.Content = "Afschrijvingen coach";
            } else if (SelectedValue == 3)
            {
                competition = true;
                coach = false;
                SortReservationLabel.Content = "Afschrijvingen wedstrijd";
            } else
            {
                coach = false;
                competition = false;
                SortReservationLabel.Content = "Afschrijvingen Persoonlijk";
            }
            DeleteAllControls();
            ShowReservations(competition, coach);
        }
    }
}
