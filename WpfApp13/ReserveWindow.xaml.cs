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
    public partial class ReserveWindow
    {
        public ReserveWindow()
        {
            InitializeComponent();
            var boats = new SampleBoatController().GetBoats();
            var reservations = new SampleReservationController().GetReservations();
            AddBoatTypeTabs(boats, reservations);
        }

        private void AddBoatTypeTabs(List<Boat> boats, List<Reservation> reservations)
        {
            foreach (var boatType in GetDifferentBoatTypes(boats))
                BoatTypeTabControl.Items.Add(new BoatTypeTabItem(GetBoatsForBoatType(boats, boatType).ToList(),
                    boatType, reservations));
        }
        private IEnumerable<Boat.BoatType> GetDifferentBoatTypes(List<Boat> boats) =>
            boats.Select(boat => boat.Type).Distinct();

        private IEnumerable<Boat> GetBoatsForBoatType(List<Boat> allBoats, Boat.BoatType boatType) =>
            allBoats.Where(boat => boat.Type == boatType);
    }
}
