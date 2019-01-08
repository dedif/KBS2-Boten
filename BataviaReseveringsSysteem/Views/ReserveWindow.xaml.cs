using System;
using Controllers;
using Models;
using System.Collections.Generic;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Reservations;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReserveWindow
    {
        public ReserveWindow(bool reservationIsForCompetition)
        {
            InitializeComponent();
            Calendar.BlackoutDates.AddDatesInPast();
            Calendar.BlackoutDates.Add(GetDisabledDatesInFuture(reservationIsForCompetition));
        }

        public void Populate(Boat boat, bool competition) => AddBoatTypeTabs(boat, competition,
            new ReservationController().GetReservationsForBoatThatAreNotDeleted(boat));

        // deze methode zorgt voor de tabbladen met de types boten bovenaan in het scherm
        private void AddBoatTypeTabs(Boat boat, bool competition, List<Reservation> reservations) =>
            BoatTypeTabControl.Children.Add(new BoatTypeTabItem(boat, competition, reservations, Calendar));

        private CalendarDateRange GetDisabledDatesInFuture(bool reservationIsForCompetition)
        {
            var now = DateTime.Now;
            var maxDate = DateTime.MaxValue;
            return reservationIsForCompetition
                ? new CalendarDateRange(now.AddYears(1).AddDays(1), maxDate)
                : new CalendarDateRange(now.AddDays(3), maxDate);
        }
    }
}