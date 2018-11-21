using ConsoleApp1;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp6;
using System.Linq;
using System.Collections.Generic;

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : Window
    {

        int y1 = 50;
        int y2 = 50;
        int i = 0;
        //Deze lijsten, bevatten alle buttens en labels
        List<Label> labelList = new List<Label>();
        List<Button> buttonList = new List<Button>();
        public Dashboard()
        {
            InitializeComponent();

            using (Database context = new Database())
            {

                GridDashboard.Margin = new Thickness(0, 0, 0, 20);
                GridDashboard.HorizontalAlignment = HorizontalAlignment.Left;
                //De reservaties van de gebruiker worden met deze methode getoond op het scherm
                ShowReservations();
            }
        }
        public void ShowReservations()
        {
            using (Database context = new Database())
            {
                //Als de gebruiker nog geen afschrijvingen heeft, dan komt dit op het scherm te staan. 
                if(context.Reservations.Count() == 0)
                {
                    NoReservationLabel.Visibility = Visibility.Visible;
                }
               
                foreach (Reservation r in context.Reservations)
                {
                    if (i % 2 == 0)
                    {

                        //Dit is voor de label aan de linkerkant van de twee rijen
                        Label l = new Label();
                        l.Content = ReservationContent(r);
                        l.Margin = new Thickness(20, y1, 50, 50);
                        l.FontSize = 16;
                        l.VerticalAlignment = VerticalAlignment.Top;
                        labelList.Add(l);
                        Button deleteButton = AddDeleteButton(20, y1 + 130, r.ReservationID);
                        Button changeButton = AddChangeButton(20, y1 + 170);
                        buttonList.Add(deleteButton);
                        buttonList.Add(changeButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        GridDashboard.Children.Add(l);
                        GridDashboard.Children.Add(deleteButton);



                        y1 = y1 + 300;
                    }
                    else if (i % 2 != 0)
                    {
                        //Hiermee maak je een label
                        Label l2 = new Label();
                        l2.Content = ReservationContent(r);
                        l2.Margin = new Thickness(500, y2, 50, 50);
                        l2.FontSize = 16;
                        l2.VerticalAlignment = VerticalAlignment.Top;
                        labelList.Add(l2);
                        Button deleteButton = AddDeleteButton(500, y2 + 130, r.ReservationID);
                        Button changeButton = AddChangeButton(500, y2 + 170);
                        buttonList.Add(deleteButton);
                        buttonList.Add(changeButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        GridDashboard.Children.Add(l2);
                        GridDashboard.Children.Add(deleteButton);


                        y2 = y2 + 300;
                    }


                    i++;
                }
            }
        }


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


                string content;
                content = "Naam : " + Name;
                content += "\nTijd: " + Date.Hour + ":" + Date.Minute;
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
                    context.Reservations.Remove(Delete);
                    context.SaveChanges();
                    //Alle oude knoppen en labels worden verwijderd van het scherm.
                    this.DeleteAllControls();
                    y1 = 50;
                    y2 = 50;
                    //De nieuwe reserveringen worden op het scherm getoond. 
                    ShowReservations();
                }

            }
        }
        //Deze methode verwijderd alle controls
        public void DeleteAllControls()
        {
            for (int i = 0; i < labelList.Count; i++)
            {
                GridDashboard.Children.Remove(labelList[i]);
            }
            for (int i = 0; i < buttonList.Count; i++)
            {
                GridDashboard.Children.Remove(buttonList[i]);
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
            this.Close();
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
