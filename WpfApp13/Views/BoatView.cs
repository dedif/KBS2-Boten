using Models;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp13
{
    public class BoatView : Grid
    {
        private Label NameLabel { get; set; }
        private Label WeightLabel { get; set; }
        private Label AmountOfRowersLabel { get; set; }
        public Label SteermanLabel { get; set; }

        public BoatView()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(150, 210, 0, 0);
            NameLabel = new Label();
            WeightLabel = new Label();
            AmountOfRowersLabel = new Label();
            SteermanLabel = new Label();
            var marginTop = 0;
            foreach (var label in new[] { NameLabel, WeightLabel, AmountOfRowersLabel, SteermanLabel })
            {
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.Margin = new Thickness(10, marginTop, 10, 10);
                Children.Add(label);
                marginTop += 10;
            }
            NoBoatSelected();
        }

        public void NoBoatSelected()
        {
            NameLabel.Content = "<geen boot geselecteerd>";
            WeightLabel.Content = AmountOfRowersLabel.Content = SteermanLabel.Content = "";
        }

        public void UpdateView(Boat boat)
        {
            NameLabel.Content = $"Naam: {boat.Name}";
            WeightLabel.Content = $"Gewicht: {boat.Weight}";
            AmountOfRowersLabel.Content = $"Aantal roeiers: {boat.NumberOfRowers}";
            SteermanLabel.Content = "Stuurman?";
            SteermanLabel.Content += boat.Steering ? "Ja" : "Nee";
        }
    }
}