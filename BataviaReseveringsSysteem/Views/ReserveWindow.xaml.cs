using Controllers;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Reservations;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReserveWindow : UserControl
    {
        public ReserveWindow()
        {
            InitializeComponent();
            var boats = new BoatController().BoatList();
            var reservations = new ReservationController().GetReservations();
            AddBoatTypeTabs(boats, reservations);
        }

        // deze methode zorgt voor de tabbladen met de types boten bovenaan in het scherm
        private void AddBoatTypeTabs(IReadOnlyCollection<Boat> boats, List<Reservation> reservations)
        {
            foreach (var boatType in GetDifferentBoatTypes(boats))
                BoatTypeTabControl.Items.Add(new BoatTypeTabItem(boatType, reservations));
        }
        private IEnumerable<Boat.BoatType> GetDifferentBoatTypes(IEnumerable<Boat> boats) =>
            boats.Select(boat => boat.Type).Distinct();

        private IEnumerable<Boat> GetBoatsForBoatType(IEnumerable<Boat> allBoats, Boat.BoatType boatType) =>
            allBoats.Where(boat => boat.Type == boatType);
    }
}