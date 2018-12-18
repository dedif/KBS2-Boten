using Models;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    public class BoatView : Grid
    {
        private const int LabelMarginTop = 30;

        private Label NameLabel { get; }
        public Label TypeLabel { get; set; }
        private Label WeightLabel { get; }
        private Label AmountOfRowersLabel { get; }
        private Label SteermanLabel { get; }
        private Label BoatLocation { get; set; }

        public BoatView()
        {

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(0, 30, 0, 0);
            NameLabel = new Label();
            TypeLabel = new Label();
            WeightLabel = new Label();
            AmountOfRowersLabel = new Label();
            BoatLocation = new Label();
            SteermanLabel = new Label();
            var marginTop = 0;
            foreach (var label in new[] { NameLabel, TypeLabel, WeightLabel, AmountOfRowersLabel, SteermanLabel, BoatLocation })
            {
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.Margin = new Thickness(10, marginTop, 10, 10);
                label.FontSize = 16;
                Children.Add(label);
                marginTop += LabelMarginTop;
            }
            NoBoatSelected();
        }

        public void NoBoatSelected()
        {
            NameLabel.Content = "<geen boot geselecteerd>";
            TypeLabel.Content = WeightLabel.Content = AmountOfRowersLabel.Content = SteermanLabel.Content = "";
        }

        public void UpdateView(Boat boat)
        {
            NameLabel.Content = $"Naam: {boat.Name}";
            TypeLabel.Content = $"Type: {boat.Type}";
            WeightLabel.Content = $"Gewicht: {boat.Weight}";
            AmountOfRowersLabel.Content = $"Aantal roeiers: {boat.NumberOfRowers}";
            SteermanLabel.Content = "Stuurman? ";
            SteermanLabel.Content += boat.Steering ? "Ja" : "Nee";
            BoatLocation.Content = $"Locatie: {boat.BoatLocation}";
        }
    }
}