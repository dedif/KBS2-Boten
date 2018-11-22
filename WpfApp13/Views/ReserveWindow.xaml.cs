using Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp13;

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
            var boats = new Boatcontroller().BoatList();
            var reservations = new ReservationController().GetReservations();
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