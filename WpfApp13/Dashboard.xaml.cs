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
                    l.Content = "Naam : ";
                    l.Content += "\nBeschrijving:";
                    l.Content += "\nTijd: ";
                    l.Content += "\nDatum: ";
                    l.Margin = new Thickness(20, y1, 50, 50);
                    l.FontSize = 16;

                    GridDashboard.Children.Add(AddButton(20, y1 + 110, "Afschrijving wijzigen"));
                    GridDashboard.Children.Add(AddButton(20, y1 + 150, "Afschrijving annuleren"));
                    GridDashboard.Children.Add(l);
                    
                    y1 = y1 + 300;
                }
               else if (i % 2 != 0)
                {
                    Label l2 = new Label();
                    l2.Content = "Naam : ";
                    l2.Content += "\nBeschrijving:";
                    l2.Content += "\nTijd: ";
                    l2.Content += "\nDatum: ";
                    l2.Margin = new Thickness(570, y2, 50, 50);
                    l2.FontSize = 16;


                    GridDashboard.Children.Add(AddButton(570, y2 + 110, "Afschrijving wijzigen"));
                    GridDashboard.Children.Add(AddButton(570, y2 + 150, "Afschrijving annuleren"));
                    GridDashboard.Children.Add(l2);
                    y2 = y2 + 300;
                }



            }




        }

        private Button AddButton(int x, int y, string omschrijving)
        {
            Button b3 = new Button();
            b3.Content = omschrijving;
            b3.HorizontalAlignment = HorizontalAlignment.Left;
            b3.VerticalAlignment = VerticalAlignment.Top;
            b3.Margin = new Thickness(x, y, 0, 0);
            b3.Height = 30;
            b3.Width = 160;
            b3.FontSize = 16;
            b3.HorizontalContentAlignment = HorizontalAlignment.Left;

            return b3;
        }
      

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
