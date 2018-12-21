using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem.Views;
using ScreenSwitcher;
using Controllers;
using BataviaReseveringsSysteem.Controllers;

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
            ShowReservations(competition);
            dashboardController.Notification(loggedUser.LastLoggedIn);

            // send email test
            //string Message = $"Hallo {loggedUser.Firstname},{Environment.NewLine}{Environment.NewLine}De boot moet vanwege zware schade worden gerepareerd.{Environment.NewLine}{Environment.NewLine}Met vriendelijke groet,{Environment.NewLine}{Environment.NewLine}Omar en de gang";
            //EmailController sendMail = new EmailController("ltzpatrick@hotmail.nl", "Uw reserveringen zijn gewijzigd omdat de boot uit de vaart is genomen.", Message);




            var getNewsMessage = (from data in context.News_Messages
                                  where data.DeletedAt == null
                                  select data).ToList();

            NewsMessageBox.ItemsSource = getNewsMessage;

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ShowNewsMessage((int)(sender as TextBox).Tag));
        }



        public void ShowReservations(bool competition)
        {
            using (var context = new DataBase())
            {

                //Geeft de reserveringen van de user
                var reservations = (
                    from data in context.Reservations
                    where data.Deleted == null
                    where data.UserId == LoginView.UserId
                    where data.Competition == competition
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
                    var changeButton = dashboardController.AddChangeButton(25, YLeft + 170);
                    ButtonList.Add(deleteButton);
                    ButtonList.Add(changeButton);

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
                    var changeButton = dashboardController.AddChangeButton(360, YRight + 170);
                    ButtonList.Add(deleteButton);
                    ButtonList.Add(changeButton);

                    //Dit voegt de label en knoppen toe aan het scherm
                    reservationsCanvas.Children.Add(l2);
                    reservationsCanvas.Children.Add(deleteButton);


                    YRight = YRight + 200;
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
        dashboardController.DeleteReservation((int)b.Tag, competition);
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

     

        private void SortReservation_Click(object sender, RoutedEventArgs e)
        {
            if (competition == false)
            {
                competition = true;
                SortReservation.Content = "Mijn afschijvingen";
                SortReservationLabel.Content = "Wedstrijd afschrijvingen";
            }
            else if (competition == true)
            {
                competition = false;
                SortReservation.Content = "Wedstrijd afschrijvingen";
                SortReservationLabel.Content = "Mijn afschijvingen";

            }
            DeleteAllControls();
            ShowReservations(competition);
        }
    }
}
