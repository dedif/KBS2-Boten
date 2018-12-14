using BataviaReseveringsSysteem.Database;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using ScreenSwitcher;
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for BoatSelectionView.xaml
    /// </summary>
    public partial class BoatSelectionView : UserControl
    {

        public BoatSelectionView()
        {
            InitializeComponent();
            Scull.IsChecked = true;
        }

        public List<Boat> boatLijst = new List<Boat>();

        private void TypeChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            BoatCombo.Items.Clear();
            if (sender == Scull) SteeringToggle.IsEnabled = true;

            var type = sender == Scull ? Boat.BoatType.Scull : (sender == Skiff ? Boat.BoatType.Skiff : Boat.BoatType.Board);

            using (DataBase context = new DataBase())
            {
                int amountOfRowersFromCombo = RowersCombo.SelectedIndex == -1 ? 0 : int.Parse(((ComboBoxItem)RowersCombo.SelectedItem).Content.ToString());
                var boats = (from data in context.Boats
                             where data.Type == type
                             where data.Steering == SteeringToggle.IsChecked
                             where data.NumberOfRowers == amountOfRowersFromCombo
                             select data).ToList();

                foreach (var item in boats) BoatCombo.Items.Add(item.Name);
            }
        }

        private void SteeringToggle_Checked(object sender, System.Windows.RoutedEventArgs e) => Refresh();

        private void SteeringToggle_Unchecked(object sender, System.Windows.RoutedEventArgs e) => Refresh();

        private void Refresh()
        {
            foreach (var type in Types.Children)
            {
                RadioButton radioButton = (RadioButton)type;
                if (radioButton.IsChecked == true) TypeChecked(radioButton, new System.Windows.RoutedEventArgs());
            }
        }

        private void RowersCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => Refresh();

        private void AnnulerenBtn_Click(object sender, System.Windows.RoutedEventArgs e) => Switcher.Switch(new Dashboard());

        private void BevestigenBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}