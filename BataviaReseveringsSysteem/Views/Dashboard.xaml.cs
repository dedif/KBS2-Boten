using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using Models;
using Controllers;
using WpfApp13;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {

        int YLeft = 50;
        int YRight = 50;
        int Count = 0;
        //Deze lijsten, bevatten alle buttens en labels
        List<Label> LabelList = new List<Label>();
        List<Button> ButtonList = new List<Button>();
        public Dashboard()
        {
            InitializeComponent();

                 GridDashboard.Margin = new Thickness(0, 0, 0, 20);
        
                //De reservaties van de gebruiker worden met deze methode getoond op het scherm
                ShowReservations();
            }
        
        public void ShowReservations()
        {
            using (Database context = new Database())
            {
                //Als de gebruiker nog geen afschrijvingen heeft, dan komt dit op het scherm te staan. 
                if(context.Reservations.Where(i => i.Deleted == false).Count() == 0)
                {
                    NoReservationLabel.Visibility = Visibility.Visible;
                }
                else 
                {
                    NoReservationLabel.Visibility = Visibility.Hidden;
                }

                if (context.Reservations.Where(i => i.Deleted == false).Count() >= 2)
                {
                    MaxReservations.Visibility = Visibility.Visible;
                     AddReservationButton.IsEnabled = false;
                }
                else
                {
                    MaxReservations.Visibility = Visibility.Hidden;
                    AddReservationButton.IsEnabled = true;
                }
                foreach (Reservation r in context.Reservations.Where(i => i.Deleted == false))
                {
                    if (Count % 2 == 0)
                    {

                        //Dit is voor de label aan de linkerkant van de twee rijen
                        Label l = new Label()
                        {
                            Content = ReservationContent(r),
                            Margin = new Thickness(20, YLeft, 50, 50),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l);
                        Button deleteButton = AddDeleteButton(20, YLeft + 130, r.ReservationID);
                        Button changeButton = AddChangeButton(20, YLeft + 170);
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
                            Content = ReservationContent(r),
                            Margin = new Thickness(500, YRight, 50, 50),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l2);
                        Button deleteButton = AddDeleteButton(500, YRight + 130, r.ReservationID);
                        Button changeButton = AddChangeButton(500, YRight + 170);
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

        //Deze methode vult de labels van de huidige reservaties
        public string ReservationContent(Reservation reservation)
        {
            using (Database context = new Database())
            {

                var ReservationBoatID = (
                    from r in context.Reservations
                    where r.ReservationID == reservation.ReservationID
                    select r.Boat.BoatID).Single();

                var Name =
                    (from boat in context.Boats
                     where boat.BoatID == ReservationBoatID
                     select boat.Name).Single();

                var Date =
                  (from r in context.Reservations
                   where r.ReservationID == reservation.ReservationID
                   select r.Start).Single();

                string minuten = Date.Minute.ToString();
                if (Date.Minute < 10)
                {
                    minuten = "0" + minuten;
                }

                string content;
                content = "Naam : " + Name;
                content += "\nTijd: " + Date.Hour + ":" + minuten;
                content += "\nDatum: " + Date.Month + "/" + Date.Day + "/" + Date.Year;

                return content;
            }
        }
        //Deze methode verwijderd de bijbehorende reservatie
        public void DeleteReservation(int id)
        {
            using (Database context = new Database())
            {
                var Delete = (
                    from r in context.Reservations
                    where r.ReservationID == id
                    select r).Single();
                //De gebruiker krijgt een controle melding.
                MessageBoxResult confirm = MessageBox.Show(
                                "Weet u zeker dat u de volgende afschrijving wilt verwijderen:\n"
                                + ReservationContent(Delete),
                                "Melding",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information);

                //Als de gebruiker de reservering wilt verwijderen.
                if (confirm == MessageBoxResult.Yes)
                {
                    //De reservering wordt uit de database verwijderd. 
                    //context.Reservations.Remove(Delete);

                    Delete.Deleted = true;
                    context.SaveChanges();
                    //Alle oude knoppen en labels worden verwijderd van het scherm.
                    this.DeleteAllControls();
                    YLeft = 50;
                    YRight = 50;
                    Count = 0;
                    //De nieuwe reserveringen worden op het scherm getoond. 
                    ShowReservations();
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
        private Button AddChangeButton(int x, int y)
        {
            //Er wordt een button aangemaakt. 
            Button Left = new Button()
            {
                Content = "Afschrijving wijzigen",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(x, y, 0, 0),
                Height = 30,
                Width = 160,
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            //De button krijgt een click event
            Left.Click += Change_Click;

            return Left;
        }


        private Button AddDeleteButton(int x, int y, int id)
        {

            Button Right = new Button()
            {
                //Er wordt een button aangemaakt. 
                Content = "Afschrijving annuleren",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(x, y, 0, 0),
                Height = 30,
                Width = 160,
                FontSize = 16,
                Tag = id,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            //De button krijgt een click event
            Right.Click += DeleteButton_Click;


            return Right;
        }




        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            DeleteReservation((int)b.Tag);
        }

        private void Change_Click(object sender, RoutedEventArgs e)
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
    }
}
