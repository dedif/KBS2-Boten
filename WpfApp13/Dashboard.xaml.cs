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

        List<Label> labelList = new List<Label>();
        List<Button> buttonList = new List<Button>();
        public Dashboard()
        {
            InitializeComponent();

            using (Database context = new Database())
            {
               Member m = new Member();
               Boat b1 = new Boat("bootl", Boat.BoatType.Board, 2, 2, false);
                Reservation res = new Reservation(b1, m, DateTime.Now, DateTime.Now);
               
                context.Reservations.Add(res);
               context.SaveChanges();

                GridDashboard.Margin = new Thickness(0, 0, 0, 20);
                GridDashboard.HorizontalAlignment = HorizontalAlignment.Left;

                ShowReservations();
            }
        }
        public void ShowReservations()
        {
            using (Database context = new Database())
            {
                
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
                        GridDashboard.Children.Add(changeButton);


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
                        GridDashboard.Children.Add(changeButton);

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

        public void DeleteReservation(int id)
        {
            using (Database context = new Database())
            {
                var Delete = (
                    from r in context.Reservations
                    where r.ReservationID == id
                    select r).Single();
                
                MessageBoxResult Annuleren = MessageBox.Show(
                                "Weet u zeker dat u de volgende afschrijving wilt verwijderen:\n"
                                + ReservationContent(Delete),
                                "Melding",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information);

                switch (Annuleren)
                {
                    case MessageBoxResult.Yes:
                        context.Reservations.Remove(Delete);
                        context.SaveChanges();
                        this.DeleteAllControls();

                        y1 = 50;
                        y2 = 50;
                        ShowReservations();
                        break;
                    case MessageBoxResult.No:
                        this.Close();
                        break;
                }
            }
        }
        public void DeleteAllControls()
        {
            for(int i = 0; i < labelList.Count; i++)
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

            Button Left = new Button();
            Left.Content = "Afschrijving wijzigen";
            Left.HorizontalAlignment = HorizontalAlignment.Left;
            Left.VerticalAlignment = VerticalAlignment.Top;
            Left.Margin = new Thickness(x, y, 0, 0);
            Left.Height = 30;
            Left.Width = 160;
            Left.FontSize = 16;
            Left.HorizontalContentAlignment = HorizontalAlignment.Left;


            Left.Click += Change_Click;

            return Left;
        }


        private Button AddDeleteButton(int x, int y, int id)
        {

            Button Right = new Button()
            {
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
