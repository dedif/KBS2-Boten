using System;
using Controllers;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BataviaReseveringsSysteem.Reservations;
using ScreenSwitcher;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReserveWindow
    {
        public ReserveWindow()
        {
            InitializeComponent();
            Calendar.SelectedDate = DateTime.Now;
        }

        public void Populate(Boat boat)
        {
//            var boats = new BoatController().GetBoatsReservableWithThisUsersDiplomasThatAreNotBroken();
//            if (boats.Count == 0)
//            {
//                MessageBox.Show("Je kan met jouw diploma's geen enkele boot afschrijven");
//                Switcher.Switch(new Dashboard());
//                return;
//            }
            var reservations = new ReservationController().GetReservationsForBoatThatAreNotDeleted(boat);
            AddBoatTypeTabs(boat, reservations);
		}
		
        // deze methode zorgt voor de tabbladen met de types boten bovenaan in het scherm
        private void AddBoatTypeTabs(Boat boat, List<Reservation> reservations) =>
            BoatTypeTabControl.Children.Add(new BoatTypeTabItem(boat, reservations, Calendar));


        private IEnumerable<Boat.BoatType> GetDifferentBoatTypes(IEnumerable<Boat> boats) =>
            boats.Select(boat => boat.Type).Distinct();

        private IEnumerable<Boat> GetBoatsForBoatType(IEnumerable<Boat> allBoats, Boat.BoatType boatType) =>
            allBoats.Where(boat => boat.Type == boatType);

        //        private List<Boat> GetBoatsReservableWithThisDiploma(List<Boat> allBoats)
        //        {
        //            
        //        }
    }
}