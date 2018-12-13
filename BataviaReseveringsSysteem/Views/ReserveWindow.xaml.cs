using System;
using Controllers;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Reservations;
using ScreenSwitcher;
using BataviaReseveringsSysteem.Database;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReserveWindow : UserControl
    {
        List<Reservation> reservations = new ReservationController().GetReservations();
        List<Boat> boats = new BoatController().GetBoatsReservableWithThisUsersDiplomasThatAreNotBroken();
        BoatTypeTabItem boattypetabItem;

        public ReserveWindow()
        {
            boattypetabItem = new BoatTypeTabItem(boats, reservations, this);
            InitializeComponent();
            Constructor();
            //fill BoatNameCombobox
            boattypetabItem.FillComboNames(boats, BoatNamesComboBox);
            ReserveGrid.Children.Add(boattypetabItem.BoatTypeTabItemCalendar);
         

        }

        public void Populate()
        {
           
            if (boats.Count == 0)
            {
                MessageBox.Show("Je kan met jouw diploma's geen enkele boot afschrijven");
                Switcher.Switch(new Dashboard());
                return;
            }
        
		}
		
  
        
       
        private IEnumerable<Boat.BoatType> GetDifferentBoatTypes(IEnumerable<Boat> boats) =>
            boats.Select(boat => boat.Type).Distinct();

        private IEnumerable<Boat> GetBoatsForBoatType(IEnumerable<Boat> allBoats, Boat.BoatType boatType) =>
            allBoats.Where(boat => boat.Type == boatType);


        // Zodra de OkBtn is aangeklikt zal de boot worden afgeschreven na messagebox dialoog bevestiging
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wilt u uw afschrijving definitief maken?",
                   "Afschrijving bevestigen",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Question) !=
               MessageBoxResult.Yes)
            {
                boattypetabItem.AddReservation();
            }
        }

        private void _reservationStartComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  boattypetabItem.OnStartComboBoxClick();
        }



        private void TimeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            boattypetabItem.FillComboNames(boats, BoatNamesComboBox);
            ////
            //Constructor(TimeCombo);
            boattypetabItem.OnDurationComboBoxClick();
        }

        public void Constructor()
        {
            // Nu worden de plannergrid, de dropdowns, de kalender en de boatview geïnitialiseerd.
            // Als je dit niet zou doen,
            // dan zouden al deze dingen leeg zijn als je het reserveringsscherm net opent en nog niets hebt aangeklikt.
            var sunriseAndSunsetTimes = boattypetabItem.GetSunriseAndSunsetTimes(DateTime.Now);

            //// De eerste slot waar het licht is. Komt de zon op om 6.24, dan is de eerste slot van 6.30 tot 6.45
            var earliestSlot = boattypetabItem.GetFirstLightSlot(sunriseAndSunsetTimes[0]);

            //// De laatste slot waar het licht is. Gaat de zon onder om 20.08, dan is de laatste slot van 19.45 tot 20.00
            var firstDarknessSlot = boattypetabItem.GetFirstDarknessSlot(sunriseAndSunsetTimes[1]);

            // Haal alle slots op
            // 1) waarbij de geselecteerde boot (we openen het reserveringsscherm net, dus de eerste boot in de botencombobox is de geselecteerde boot) al afgeschreven is
            // 2) al voorbij zijn (maar waar de zon al wel op was)
            // 3) te ver in de toekomst zijn om geclaimd te kunnen worden (maar waar de zon nog niet onder is)

            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                boattypetabItem.GetClaimedBrokenPastAndTooDistantSlotsForThisDayAndBoat(DateTime.Now, DateTime.Now, (string)BoatNamesComboBox.SelectedValue, earliestSlot,
                    firstDarknessSlot);

            // Vul de PlannerGrid met de slots voor deze dag.
            // Alles vòòr de eerste en na de laatste slot wordt donkerblauw.
            // Alle slots die al geclaimd zijn, voorbij zijn of nog niet gereserveerd mogen worden, worden grijs

            boattypetabItem.PlannerGrid.Populate(earliestSlot, firstDarknessSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat);
       //     ReserveGrid.Children.Add(boattypetabItem.PlannerGrid);




            // Vul de reservationstartcombobox met de beschikbare starttijden voor een afschrijving.
            // Om een starttijd te mogen kiezen, moet de slot van de starttijd aan de volgende voorwaarden voldoen:
            // 1) Het slot moet na zonsopgang zijn
            // 2) Het slot moet vòòr zonsondergang zijn
            // 3) Het slot is nog niet afgeschreven
            // 4) Het slot begint niet in het verleden

            boattypetabItem.PopulateStartTimeComboBox(DateTime.Now, earliestSlot, firstDarknessSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat);

        }


    }
}