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
            AllowedCompetition();
            Scull.IsChecked = true;
        }

        private Boat _boat;

        private bool _competition;
        private bool _coach;
        //Deze methode kijkt of je ook voor wedstrijden mag afschrijven
        private void AllowedCompetition()
        {
            using (DataBase context = new DataBase())
            {
                var RolID = (from data in context.User_Roles
                             where data.UserID == LoginView.UserId
                             select data.RoleID).ToList();


                //Als de gebruiker een coach, wedstrijdcommisaris of het bestuur is dan
                if (RolID.Contains(2) || RolID.Contains(5) || RolID.Contains(3))
                {
                    SelectReservation.Visibility = Visibility.Visible;
                    KindReservationLabel.Visibility = Visibility.Visible;
                    SelectReservation.SelectedIndex = 1;
                   
                    //De afschrijvingen voor persoonlijk gebruik van een wedstrijdcommisaris en coach
                    var ReservationsPersonal = (from data in context.Reservations
                                                where data.UserId == LoginView.UserId
                                                where data.Coach == false
                                                where data.Competition == false
                                                where data.Deleted == null
                                                select data).ToList();

                    //De afschrijvingen voor de lessen van de coach
                    var ReservationsCoach = (from data in context.Reservations
                                                where data.UserId == LoginView.UserId
                                                where data.Coach == true
                                                where data.Deleted == null
                                                select data).ToList();

                    //De afschrijvingen voor de wedsrijden van de wedstrijdcommisaris
                    var ReservationsCompitions = (from data in context.Reservations
                                             where data.UserId == LoginView.UserId
                                             where data.Competition == true
                                             where data.Deleted == null
                                             select data).ToList();

                    //De wedstrijdcommisaris/coach mag maximaal 2 afschrijvingen voor de zichzelf afschrijven
                    if (ReservationsPersonal.Count >= 2)
                    {
                        SelectReservation.Items.Remove((ComboBoxItem)Normal);
                        MaxReservation.Visibility = Visibility.Visible;
                    }
                    //De coach mag maximaal 6 afschrijvingen voor de lessen afschrijven
                    if (ReservationsCoach.Count >= 6)
                    {
                        SelectReservation.Items.Remove((ComboBoxItem)Coach);
                     
                    }
                    //De wedstrijdcommisaris mag maximaal 6 afschrijvingen voor de wedstrijden afschrijven
                    if (ReservationsCompitions.Count >= 6)
                    {
                        SelectReservation.Items.Remove((ComboBoxItem)Competition);
                     
                    }

                    //Als de gebruiker een coach is dan mag hij afschrijving maken voor lessen.
                    if (!RolID.Contains(2))
                    {
                        SelectReservation.Items.Remove((ComboBoxItem)Coach);

                    }
                    //Als de gebruiker een wedstrijdcommisaris is dan mag hij afschrijvingen maken voor wedstrijden.
                    if (!RolID.Contains(3))
                    {
                        SelectReservation.Items.Remove((ComboBoxItem)Competition);

                    }

                }

            }
        }
        private void TypeChecked(object sender, RoutedEventArgs e)
        {


            if (Equals(sender, Skiff))
            {
                RowersCombo.IsEnabled = false;
                RowersCombo.SelectedItem = oneRower;
                SteeringToggle.IsEnabled = false;
            }
            else
            {
                if (Equals(RowersCombo.SelectedItem, oneRower))
                {
                    RowersCombo.SelectedIndex = 1;
                }
                RowersCombo.IsEnabled = true;
                oneRower.IsEnabled = false;
                SteeringToggle.IsEnabled = true;

            }

            BoatCombo.Items.Clear();

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
                             where data.Broken == false
                             select data).ToList().Distinct();

                var boatsAvailableForUser = (from data in context.Boats
                             join d in context.Boat_Diplomas on data.BoatID equals d.BoatID
                             join u in context.User_Diplomas on d.DiplomaID equals u.DiplomaID
                             where data.BoatID == d.BoatID
                             where d.DiplomaID == u.DiplomaID
                             where data.AvailableAt <= DateTime.Now
                             where data.Deleted == false
                             where data.Broken == false
                             select data).ToList();

                if (boatsAvailableForUser.Count < 1)
                {
                    //Als de gebruiker nit de juiste diploma's heeft
                    MessageBoxResult NoBoatsAvailable = MessageBox.Show(
                        "U heeft niet de juiste diploma's om boten te kunnen reserveren",
                        "Melding",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    // Dan kan hij nergens meer op klikken.
                    Skiff.IsEnabled = false;
                    Scull.IsEnabled = false;
                    Board.IsEnabled = false;
                    SteeringToggle.IsEnabled = false;
                    RowersCombo.IsEnabled = false;
                    SelectReservation.IsEnabled = false;
                    BoatCombo.IsEnabled = false;
                }

                //Deze query haalt alle boten uit de database die licht beschadigd zijn
                var DamagedBoats = (from data in context.Damages
                                    where data.Status == "Lichte schade"
                                    select data.BoatID).ToList();

                foreach (var item in boats)
                {
                    //Als de boot licht beschadigd is dan wordt dit vermeld bij het selecteren van een boot
                    if (DamagedBoats.Contains(item.BoatID))
                    {
                        BoatCombo.Items.Add(item.Name + " (" + item.Weight +  "kg) (beschadigd)");
                    }
                    else
                    {
                        BoatCombo.Items.Add(item.Name +  " (" + item.Weight + "kg)");
                    }
                }
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
            int SelectedValue = int.Parse(((ComboBoxItem)SelectReservation.SelectedItem).Tag.ToString());
            if (SelectedValue == 2)
            {
                _competition = false;
                _coach = true;
            }
            else if (SelectedValue == 3)
            {
                _competition = true;
                _coach = false;
            }
            else
            {
                _coach = false;
                _competition = false;
            }
            var reserveWindow = new ReserveWindow(_competition, _coach, _boat);
                Switcher.Switch(reserveWindow);
                reserveWindow.Populate(_boat, _competition, _coach);
            
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

        private Boat GetBoatFromBoatComboBox()
        {
            //Je pakt alleen de naam van de boot, die de gebruiker selecteerd.
            string BoatName = BoatCombo.SelectedItem.ToString().Substring(0, BoatCombo.SelectedItem.ToString().IndexOf(" ("));
           return new BoatController().GetBoatWithName(BoatName);
        }

        private bool IsBoatSelected() => BoatCombo.SelectedIndex != -1;

        private void EnableConfirmButtonIfBoatIsSelected() => BevestigenBtn.IsEnabled = BoatCombo.SelectedIndex != -1;

   
    }
}