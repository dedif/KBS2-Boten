using System;
using BataviaReseveringsSysteem.Database;
using Models;
using System.Linq;
using System.Windows.Controls;
using Controllers;
using ScreenSwitcher;
using Views;
using System.Windows;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for BoatSelectionView.xaml
    /// </summary>
    public partial class BoatSelectionView
    {

        public BoatSelectionView()
        {
            InitializeComponent();
            Scull.IsChecked = true;
        }

        private Boat _boat;

        private void TypeChecked(object sender, RoutedEventArgs e)
        {
            BoatCombo.Items.Clear();
            SteeringToggle.IsEnabled = Equals(sender, Scull) || Equals(sender, Board);

            if(Equals(sender, Skiff))
            {
                RowersCombo.IsEnabled = false;
                RowersCombo.SelectedItem = oneRower;
            }
            else
            {
                RowersCombo.IsEnabled = true;
                oneRower.IsEnabled = false;
            }

            var type = Equals(sender, Scull) ? Boat.BoatType.Scull : Equals(sender, Skiff) ? Boat.BoatType.Skiff : Boat.BoatType.Board;

            using (var context = new DataBase())
            {
                var amountOfRowersFromCombo = RowersCombo.SelectedIndex == -1 ? 0 : int.Parse(((ComboBoxItem)RowersCombo.SelectedItem).Content.ToString());
                var boats = (from data in context.Boats
                             join d in context.Boat_Diplomas on data.BoatID equals d.BoatID
                             join u in context.User_Diplomas on d.DiplomaID equals u.DiplomaID
                             where data.BoatID == d.BoatID
                             where d.DiplomaID == u.DiplomaID
                             where data.Type == type
                             where data.Steering == SteeringToggle.IsChecked
                             where data.NumberOfRowers == amountOfRowersFromCombo
                             where data.AvailableAt <= DateTime.Now
                             where data.Deleted == false
                             select data).ToList().Distinct();

                foreach (var item in boats) BoatCombo.Items.Add(item.Name);
            }
        }

        private void SteeringToggle_Checked(object sender, RoutedEventArgs e) => Refresh();

        private void SteeringToggle_Unchecked(object sender, RoutedEventArgs e) => Refresh();

        private void Refresh()
        {
            foreach (var type in Types.Children)
            {				
                RadioButton radioButton = (RadioButton)type;
                if (radioButton.IsChecked == true) TypeChecked(radioButton, new RoutedEventArgs());
            }
        }

        private void RowersCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => Refresh();

        private void AnnulerenBtn_Click(object sender, RoutedEventArgs e) => Switcher.Switch(new Dashboard());

        private void BevestigenBtn_Click(object sender, RoutedEventArgs e)
        {
            var reserveWindow = new ReserveWindow();
            Switcher.Switch(reserveWindow);
            reserveWindow.Populate(_boat);
        }

        private void BoatCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsBoatSelected())
            {
                BevestigenBtn.IsEnabled = true;
                _boat = GetBoatFromBoatComboBox();
            }
            else
            {
                BevestigenBtn.IsEnabled = false;
                _boat = null;
            }
        }

        private Boat GetBoatFromBoatComboBox() =>
            new BoatController().GetBoatWithName(BoatCombo.SelectedItem.ToString());

        private bool IsBoatSelected() => BoatCombo.SelectedIndex != -1;

        private void EnableConfirmButtonIfBoatIsSelected() => BevestigenBtn.IsEnabled = BoatCombo.SelectedIndex != -1;
    }
}