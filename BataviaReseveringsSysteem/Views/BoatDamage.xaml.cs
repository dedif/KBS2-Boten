using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for BoatDamage.xaml
    /// </summary>
    public partial class BoatDamage : UserControl
    {
        DataBase context = new DataBase();
        public BoatDamage()
        {
            InitializeComponent();

            var NameBoats = (from data in context.Boats
                            select data.Name).ToList();

            foreach (string name in NameBoats) {
                NameboatCombo.Items.Add(name);
           }
            NameboatCombo.SelectedIndex = 0;

            LightDamageRadioButton.IsChecked = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty(DescriptionBox.Text)) { }
            else
            {
                MessageBoxResult Melding = MessageBox.Show(
                            "Weet u zeker dat u deze schade wilt melden?",
                            "Melding",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                switch (Melding)
                {
                    case MessageBoxResult.Yes:

                        var Boat = (from data in context.Boats
                                    where data.Name == NameboatCombo.Text
                                    select data).Single();

                        string status = null;
                        if (LightDamageRadioButton.IsChecked == true)
                        {
                            status = "Lichte schade";
                        }
                        else if (HeavyDamageRadioButton.IsChecked == true)
                        {
                            status = "Zware schade";
                            Boat.Broken = true;
                        }

                        Damage damage = new Damage(LoginView.LoggedUser.PersonID, Boat.BoatID, DescriptionBox.Text, status);

                        context.Damages.Add(damage);
                        context.SaveChanges();

                        Switcher.Switch(new Dashboard());
                        break;

                    case MessageBoxResult.No:
                        break;

                }
            }
        }

        private void HeavyDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Visible;
        }

        private void LightDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Hidden;
        }

        public bool IsEmpty(string beschrijving)
        {
            if (string.IsNullOrWhiteSpace(beschrijving))
            {
                MessageLabel.Visibility = Visibility.Visible;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
