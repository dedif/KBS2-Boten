using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ConsoleApp1;

namespace WpfApp6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReserveWindow : Window
    {
        public ReserveWindow()
        {
            InitializeComponent();
            var boats = new SampleBoatController().GetBoats();
            var reservations = new SampleReservationController().GetReservations();
            addBoatTypeTabs(boats, reservations);
        }

        private void addBoatTypeTabs(List<Boat> boats, List<Reservation> reservations)
        {
            foreach (var boatType in GetDifferentBoatTypes(boats))
            {
                //var reservationsForBoatType = GetReservationsForBoatType(reservations, boatType);
                BoatTypeTabControl.Items.Add(new BoatTypeTabItem(boatType, reservations));
            }
        }
        private IEnumerable<Boat.BoatType> GetDifferentBoatTypes(List<Boat> boats) =>
            boats.Select(boat => boat.Type).Distinct();

        //private IEnumerable<Reservation> GetReservationsForBoatType(List<Reservation> reservations, string boatType) =>
        //    reservations.Where(r => r.Boat.Type.Equals(boatType));

    }
}
