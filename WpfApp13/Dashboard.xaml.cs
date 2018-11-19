using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : Window
    {

        int y1 = 50;
        int y2 = 50;
       
        public Dashboard()
        {
            InitializeComponent();
            GridDashboard.Margin = new Thickness(0, 0, 0, 20);

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    Label l = new Label();
                    l.Content = "naam : ";
                    l.Content += "\nbeschrijving:";
                    l.Content += "\ntijd: ";
                    l.Content += "\ndatum: ";
                    l.Margin = new Thickness(20, y1, 50, 50);
                    l.FontSize = 18;


                    Button b1 = new Button();
                    b1.Content = "afschrijving wijzigen";
                    b1.HorizontalAlignment = HorizontalAlignment.Left;
                    b1.VerticalAlignment = VerticalAlignment.Top;
                    b1.Margin = new Thickness(20, y1 + 120, 0, 0);
                    b1.Height = 20;
                    b1.Width = 140;

                    Button b2 = new Button();
                    b2.Content = "afschrijving annuleren";
                    b2.HorizontalAlignment = HorizontalAlignment.Left;
                    b2.VerticalAlignment = VerticalAlignment.Top;
                    b2.Margin = new Thickness(20, y1 + 150, 0, 0);
                    b2.Height = 20;
                    b2.Width = 140;


                    GridDashboard.Children.Add(b1);
                    GridDashboard.Children.Add(b2);
                    GridDashboard.Children.Add(l);
                    
                    y1 = y1 + 300;
                }
               else if (i % 2 != 0)
                {
                    Label l2 = new Label();
                    l2.Content = "naam : ";
                    l2.Content += "\nbeschrijving:";
                    l2.Content += "\ntijd: ";
                    l2.Content += "\ndatum: ";
                    l2.Margin = new Thickness(570, y2, 50, 50);
                    l2.FontSize = 18;

                    Button b3 = new Button();
                    b3.Content = "afschrijving wijzigen";
                    b3.HorizontalAlignment = HorizontalAlignment.Left;
                    b3.VerticalAlignment = VerticalAlignment.Top;
                    b3.Margin = new Thickness(570, y2 + 120, 0, 0);
                    b3.Height = 20;
                    b3.Width = 140;

                    Button b4 = new Button();
                    b4.Content = "afschrijving annuleren";
                    b4.HorizontalAlignment = HorizontalAlignment.Left;
                    b4.VerticalAlignment = VerticalAlignment.Top;
                    b4.Margin = new Thickness(570, y2 + 150, 0, 0);
                    b4.Height = 20;
                    b4.Width = 140;


                    GridDashboard.Children.Add(b3);
                    GridDashboard.Children.Add(b4);
                    GridDashboard.Children.Add(l2);
                    y2 = y2 + 300;
                }



            }




        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
