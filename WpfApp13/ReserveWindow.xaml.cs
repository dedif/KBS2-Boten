using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ConsoleApp1;
using WpfApp13;

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
            var boats = new Boatcontroller().BoatList();
            var reservations = new ReservationController().GetReservations();
            addBoatTypeTabs(boats, reservations);
        }

        private void addBoatTypeTabs(List<Boat> boats, List<Reservation> reservations)
        {
            foreach (var boatType in GetDifferentBoatTypes(boats))
                BoatTypeTabControl.Items.Add(new BoatTypeTabItem(boatType, reservations));
        }
        private IEnumerable<Boat.BoatType> GetDifferentBoatTypes(List<Boat> boats) =>
            boats.Select(boat => boat.Type).Distinct();

        // Method trims ComboBoxItem string to int without hours || without minutes

        

        

        //private void AfschrijvenBtn_Click(object sender, RoutedEventArgs e)
        //{ 
        //    Boat b1 = new Boat();
        //    using (Database context = new Database())
        //    {
        //        //var x = BoatTypeTabControl.SelectedItem;
        //        //BoatTypeTabItem y = (BoatTypeTabItem)x;

        //        Reservation rs1 = new Reservation(b1, new Member(), /*y.Calendar.SelectedDate.Value*/DateTime.Now, DateTime.Now.AddMinutes(CalculateQuarterFromComboBox()));

        //        context.Reservations.Add(rs1);

        //        context.SaveChanges();
        //    }
        //}

        //private IEnumerable<Reservation> GetReservationsForBoatType(List<Reservation> reservations, string boatType) =>
        //    reservations.Where(r => r.Boat.Type.Equals(boatType));

    }
}
