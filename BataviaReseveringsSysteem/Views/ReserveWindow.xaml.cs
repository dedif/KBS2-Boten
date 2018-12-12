using System;
using Controllers;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Reservations;
using ScreenSwitcher;

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
        }

        public void Populate()
        {
            var boats = new BoatController().GetBoatsReservableWithThisUsersDiplomasThatAreNotBroken();
            Console.WriteLine(boats);
            if (boats.Count == 0)
            {
                MessageBox.Show("Je kan met jouw diploma's geen enkele boot afschrijven");
                Switcher.Switch(new Dashboard());
                return;
            }
            var reservations = new ReservationController().GetReservations();
            AddBoatTypeTabs(boats, reservations);
		}
		
        // deze methode zorgt voor de tabbladen met de types boten bovenaan in het scherm
        private void AddBoatTypeTabs(List<Boat> boats, List<Reservation> reservations)
        {
            BoatTypeTabControl.Items.Add(new BoatTypeTabItem(boats, reservations));
        }
        
       
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