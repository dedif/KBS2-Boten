using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using Models;
using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem.Views;
using ScreenSwitcher;
using Controllers;


namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard
    {
        public int YLeft = 100;
        public int YRight = 100;
        public int Count;
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



        }

   

        public void ShowReservations()
        {
            using (var context = new DataBase())
            {

                //Geeft de reserveringen van de user
                var reservations = (
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
                if (reservations.Count >= MaxReservationUser)

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
            foreach (var r in reservations)

                {
                    if (Count % 2 == 0)
                    {

                        //Dit is voor de label aan de linkerkant van de twee rijen
                        var l = new Label
                        {
                            Content = dashboardController.ReservationContent(r),
                            Margin = new Thickness(20, YLeft, 50, 50),
                           FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l);
                        var deleteButton = dashboardController.AddDeleteButton(20, YLeft + 130, r.ReservationID);
                        var changeButton = dashboardController.AddChangeButton(20, YLeft + 170);
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
                        var l2 = new Label
                        {
                            Content = dashboardController.ReservationContent(r),
                            Margin = new Thickness(500, YRight, 50, 50),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l2);
                        var deleteButton = dashboardController.AddDeleteButton(500, YRight + 130, r.ReservationID);
                        var changeButton = dashboardController.AddChangeButton(500, YRight + 170);
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
            foreach (var t in LabelList)
            {
                GridDashboard.Children.Remove(t);
            }

            foreach (var t in ButtonList)
            {
                GridDashboard.Children.Remove(t);
            }
        }



        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            dashboardController.DeleteReservation((int)b.Tag);
        }

        public void Change_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        //            var reserveWindow = new ReserveWindow();
        //            Switcher.Switch(reserveWindow);
        //            reserveWindow.Populate();
        private void AddReservationButton_Click(object sender, RoutedEventArgs e) =>
            Switcher.Switch(new BoatSelectionView());
    }
}
