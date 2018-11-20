using ConsoleApp1;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp6;
using System.Linq;


namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : Window
    {

        int y1 = 50;
        int y2 = 50;
        int j = 1;
        public Dashboard()
        {
            InitializeComponent();

            using (Database context = new Database())
            {
                Boat b = new Boat("help", Boat.BoatType.Board, 4, 21, true);
                Reservation reservering = new Reservation(b, new Member(), DateTime.Now, DateTime.Now);
          
                context.Reservations.Add(reservering);
                context.SaveChanges();
               



                GridDashboard.Margin = new Thickness(0, 0, 0, 20);
                GridDashboard.HorizontalAlignment = HorizontalAlignment.Left;
                
                

                for (int i = 1; i < context.Reservations.Count() + 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        var ReservationBoatID =(
                            from r in context.Reservations
                            where r.ReservationID == i
                            select r.Boat.BoatID).Single();

                    
                        var Name =
                            (from boat in context.Boats
                             where boat.BoatID == ReservationBoatID
                             select boat.Name).Single();



                        Label l = new Label();
                        l.Content = "Naam :" + Name;
                        l.Content += "\nTijd: ";
                        l.Content += "\nDatum: ";
                        l.Margin = new Thickness(20, y2, 50, 50);
                        l.FontSize = 16;
                        l.VerticalAlignment = VerticalAlignment.Top;
                    //    l.HorizontalAlignment = HorizontalAlignment.Left;
                        GridDashboard.Children.Add(l);
                        GridDashboard.Children.Add(AddButtonLinks(y2 + 130, "Afschrijving wijzigen"));
                        GridDashboard.Children.Add(AddButtonLinks(y2 + 170, "Afschrijving annuleren"));
                        

                        y2 = y2 + 300;
                    }
                    else if (i % 2 != 0)
                    {

                        var ReservationBoatID = (
                           from r in context.Reservations
                           where r.ReservationID == i
                           select r.Boat.BoatID).Single();


                        var Name =
                            (from boat in context.Boats
                             where boat.BoatID == ReservationBoatID
                             select boat.Name).Single();


                        Label l2 = new Label();
                        l2.Content = "Naam : " + Name;
                        l2.Content += "\nTijd: ";
                        l2.Content += "\nDatum: ";
                        l2.Margin = new Thickness(500, y1, 50, 50);
                        l2.FontSize = 16;
                        l2.VerticalAlignment = VerticalAlignment.Top;
                      //  l2.HorizontalAlignment = HorizontalAlignment.Left;

                        GridDashboard.Children.Add(l2);
                        GridDashboard.Children.Add(AddButtonRechts(y1 + 130, "Afschrijving wijzigen"));
                        GridDashboard.Children.Add(AddButtonRechts(y1 + 170, "Afschrijving annuleren"));

                        y1 = y1 + 300;
                    }



                }


            }

        }

        private Button AddButtonLinks(int y, string omschrijving)
        {
           
                Button Links = new Button();
                Links.Content = omschrijving;
                Links.HorizontalAlignment = HorizontalAlignment.Left;
                Links.VerticalAlignment = VerticalAlignment.Top;
                Links.Margin = new Thickness(20, y, 0, 0);
                Links.Height = 30;
                Links.Width = 160;
                Links.FontSize = 16;
                Links.HorizontalContentAlignment = HorizontalAlignment.Left;

                Links.Click += Button2_Click;

                return Links;
            }


        private Button AddButtonRechts(int y, string omschrijving)
            {
              
                    Button Rechts = new Button();
                    Rechts.Content = omschrijving;
                    Rechts.HorizontalAlignment = HorizontalAlignment.Left;
                    Rechts.VerticalAlignment = VerticalAlignment.Top;
                    Rechts.Margin = new Thickness(500, y, 0, 0);
                    Rechts.Height = 30;
                    Rechts.Width = 160;
                    Rechts.FontSize = 16;
                    Rechts.HorizontalContentAlignment = HorizontalAlignment.Left;

                    Rechts.Click += Button_Click;

                    return Rechts;
                }

   
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
