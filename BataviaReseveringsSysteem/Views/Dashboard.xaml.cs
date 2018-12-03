using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using Models;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System;
using Controllers;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {

       public int YLeft = 50;
        public int YRight = 50;
        public int Count = 0;
        public int MaxReservationUser = 2;
        //Deze lijsten, bevatten alle buttens en labels
        public List<Label> LabelList = new List<Label>();
        public List<Button> ButtonList = new List<Button>();
        DataBase context = new DataBase();
        DashboardController dashboardController;
        public Dashboard()
        {
            InitializeComponent();
            GridDashboard.VerticalAlignment = VerticalAlignment.Top;
            GridDashboard.Margin = new Thickness(50, 0, 50, 20);

            var loggedUser = (from data in context.Users
                           where data.PersonID == LoginView.LoggedUserID
                           select data).Single();

            NameLabel.Content = loggedUser.Firstname + " " + loggedUser.Lastname;


            dashboardController = new DashboardController(this);
          
            var rol = (from data in context.MemberRoles
                           where data.PersonID == LoginView.LoggedUserID
                           select data.RoleID).ToList();

                if (rol.Contains(6))
                {
                    MaxReservationUser = 2;
                    AddBoatButton.Visibility = Visibility.Visible;
                    UserListButton.Visibility = Visibility.Visible;
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


        }



        public void ShowReservations()
        {
            using (DataBase context = new DataBase())
            {
                //Als de gebruiker nog geen afschrijvingen heeft, dan komt dit op het scherm te staan. 
                if (context.Reservations.Where(i => i.Deleted == null).Count() == 0)
                {
                    NoReservationLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    NoReservationLabel.Visibility = Visibility.Hidden;
                }

                if (context.Reservations.Where(i => i.Deleted == null).Count() >= MaxReservationUser)
                {
                    MaxReservations.Visibility = Visibility.Visible;
                    //AddReservationButton.IsEnabled = false;
                }
                else
                {
                    MaxReservations.Visibility = Visibility.Hidden;
                    //AddReservationButton.IsEnabled = true;
                }

                var OrderedReservations = (from data in context.Reservations
                                           where data.Deleted == null
                                           where data.UserID == LoginView.LoggedUserID
                                           orderby data.Start
                                           select data).ToList();

                foreach (Reservation r in OrderedReservations)
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
            Switcher.Switch(new ReserveWindow());

        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void UserListButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserList());
        }

        private void AddBoatButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        private void DamageButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamage());
        }
    }
}
