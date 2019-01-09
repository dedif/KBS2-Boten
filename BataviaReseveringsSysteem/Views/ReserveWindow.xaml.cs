using System;
using Controllers;
using Models;
using System.Collections.Generic;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Reservations;
using BataviaReseveringsSysteem.Database;
using System.Linq;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReserveWindow
    {
        public ReserveWindow(bool reservationIsForCompetition, bool coach, Boat boat)
        {
            InitializeComponent();
            Calendar.BlackoutDates.AddDatesInPast();
            Calendar.BlackoutDates.Add(GetDisabledDatesInFuture(reservationIsForCompetition));
            Calendar.BlackoutDates.Add(GetDisabledDatesInFuture(coach));

            using (DataBase context = new DataBase())
            {

                var boatRepare = (from data in context.Boats
                                  join a in context.Damages on data.BoatID equals a.BoatID
                                  where data.Name == boat.Name
                                  select a).ToList();

                foreach (var b in boatRepare)
                {
                    if (b.TimeOfOccupyForFix != DateTime.Today)
                    {
                        if (b.TimeOfFix != DateTime.Today)
                        {
                            Calendar.BlackoutDates.Add(new CalendarDateRange(b.TimeOfOccupyForFix.Value.Date, b.TimeOfFix.Value.Date));
                        }

                    }
                }
            }
      
        }

        public void Populate(Boat boat, bool competition, bool coach) => AddBoatTypeTabs(boat, competition, coach,
            new ReservationController().GetReservationsForBoatThatAreNotDeleted(boat));

        // deze methode zorgt voor de tabbladen met de types boten bovenaan in het scherm
        private void AddBoatTypeTabs(Boat boat, bool competition, bool coach, List<Reservation> reservations) =>
            BoatTypeTabControl.Children.Add(new BoatTypeTabItem(boat, competition, coach, reservations, Calendar));

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