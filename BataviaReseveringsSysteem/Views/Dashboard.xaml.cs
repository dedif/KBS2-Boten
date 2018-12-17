﻿using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using Models;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System;
using Controllers;
using BataviaReseveringsSysteem.Views;
using BataviaReseveringsSysteem.Controllers;
using System.Net.Mail;
using System.Text;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public int YLeft = 100;
        public int YRight = 100;
        public int Count = 0;
        public int MaxReservationUser = 2;
        //Deze lijsten, bevatten alle buttens en labels
        public List<Label> LabelList = new List<Label>();
        public List<Button> ButtonList = new List<Button>();
        DataBase context = new DataBase();
        DashboardController dashboardController;
        public static NavigationView navigationview;
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

            if (rol.Contains(5))
            {
                MaxReservationUser = 2;
            }

            if (rol.Contains(3))
            {
                MaxReservationUser = 8;
            }

            if (rol.Contains(4))
            {
                MaxReservationUser = int.MaxValue;
            }

            if (rol.Contains(5))
            {
                MaxReservationUser = int.MaxValue;
            }

            //De reservaties van de gebruiker worden met deze methode getoond op het scherm
            ShowReservations();
            dashboardController.Notification(loggedUser.LastLoggedIn);

            // send email test
            //string Message = $"Hallo {loggedUser.Firstname},{Environment.NewLine}{Environment.NewLine}De boot moet vanwege zware schade worden gerepareerd.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}Omar en de gang";
            //EmailController sendMail = new EmailController("ltzpatrick@hotmail.nl", "Uw reserveringen zijn gewijzigd omdat de boot uit de vaart is genomen.", Message);

        }

   

        public void ShowReservations()
        {
            using (DataBase context = new DataBase())
            {

                //Geeft de reserveringen van de user
                var Reservations = (
                    from data in context.Reservations
                    where data.Deleted == null
                    where data.UserId == LoginView.UserId
                    select data).ToList();

                //Als de gebruiker nog geen afschrijvingen heeft, dan komt dit op het scherm te staan. 

                if (Reservations.Count() == 0)

                {
                    NoReservationLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    NoReservationLabel.Visibility = Visibility.Hidden;
                }

                //Als de gebruiker het maximale aantal afschrijvingen heeft bereikt, mag hij geen boten meer afschrijven
                if (Reservations.Count() >= MaxReservationUser)

                {
                    MaxReservations.Visibility = Visibility.Visible;
                    AddReservationButton.IsEnabled = false;
                    navigationview.MakeAddReservationInvisible(false);
                }
                else
                {
                    MaxReservations.Visibility = Visibility.Hidden;
                    AddReservationButton.IsEnabled = true;
                    navigationview.MakeAddReservationInvisible(true);
                }
            foreach (Reservation r in Reservations)

                {
                    if (Count % 2 == 0)
                    {

                        //Dit is voor de label aan de linkerkant van de twee rijen
                        Label l = new Label()
                        {
                            Content = dashboardController.ReservationContent(r),
                            Margin = new Thickness(20, YLeft, 50, 50),
                           FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l);
                        Button deleteButton = dashboardController.AddDeleteButton(20, YLeft + 130, r.ReservationID);
                        Button changeButton = dashboardController.AddChangeButton(20, YLeft + 170);
                        ButtonList.Add(deleteButton);
                        ButtonList.Add(changeButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        GridDashboard.Children.Add(l);
                        GridDashboard.Children.Add(deleteButton);


                        YLeft = YLeft + 300;
                    }
                    else if (Count % 2 != 0)
                    {
                        //Hiermee maak je een label
                        Label l2 = new Label()
                        {
                            Content = dashboardController.ReservationContent(r),
                            Margin = new Thickness(500, YRight, 50, 50),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l2);
                        Button deleteButton = dashboardController.AddDeleteButton(500, YRight + 130, r.ReservationID);
                        Button changeButton = dashboardController.AddChangeButton(500, YRight + 170);
                        ButtonList.Add(deleteButton);
                        ButtonList.Add(changeButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        GridDashboard.Children.Add(l2);
                        GridDashboard.Children.Add(deleteButton);


                        YRight = YRight + 300;
                    }


                    Count++;

                    var getNewsMessage = (from data in context.News_Messages
                                          where data.DeletedAt == null
                                          select data).ToList();
                    int top = 10;
                    foreach (var news in getNewsMessage)
                    {
                        Label l3 = new Label()
                        {
                            Content = $"{news.Title} {news.CreatedAt}",
                            Margin = new Thickness(0, top, 0, 0),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        NewsList.Children.Add(l3);
                        top += 10;
                    }
                }
           
            }
        }

        //Deze methode verwijderd alle controls
        public void DeleteAllControls()
        {
            for (int i = 0; i < LabelList.Count; i++)
            {
                GridDashboard.Children.Remove(LabelList[i]);
            }
            for (int i = 0; i < ButtonList.Count; i++)
            {
                GridDashboard.Children.Remove(ButtonList[i]);
            }
        }



        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            dashboardController.DeleteReservation((int)b.Tag);
        }

        public void Change_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {
            var reserveWindow = new ReserveWindow();
            Switcher.Switch(reserveWindow);
            reserveWindow.Populate();
        }
    }
}
