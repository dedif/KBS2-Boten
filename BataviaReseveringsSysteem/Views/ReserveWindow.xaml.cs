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
            Calendar.BlackoutDates.Add(GetDisabledDatesInFuture(reservationIsForCompetition, coach));

            // Schakel alle data uit waarop de gekozen boot wordt gerepareerd.
            using (var context = new DataBase())
            {
                var boatRepair = (from data in context.Boats
                                  join a in context.Damages on data.BoatID equals a.BoatID
                                  where data.Name == boat.Name
                                  select a).ToList();
                foreach (var b in boatRepair)
                {
                    if (b.TimeOfOccupyForFix == DateTime.Today) continue;
                    if (b.TimeOfFix != DateTime.Today)
                        Calendar.BlackoutDates.Add(new CalendarDateRange(b.TimeOfOccupyForFix.Value.Date,
                            b.TimeOfFix.Value.Date));
                }
            }
        }

        // Dit kan om technische redenen niet in de constructor.
        public void Populate(Boat boat, bool competition, bool coach) => AddBoatTypeTabs(boat, competition, coach,
            new ReservationController().GetReservationsForBoatThatAreNotDeleted(boat));

        // deze methode zorgt voor de tabbladen met de types boten bovenaan in het scherm
        private void AddBoatTypeTabs(Boat boat, bool competition, bool coach, List<Reservation> reservations) =>
            BoatTypeTabControl.Children.Add(new BoatTypeTabItem(boat, competition, coach, reservations, Calendar));

        // Geef de data die te ver in de toekomst zijn om er te mogen reserveren,
        // en dus onklikbaar worden in de kalender.
        // Is de afschrijving voor een wedstrijd? 1 jaar na dato is de laatste dag
        // Is de afschrijving voor een training? 7 dagen na dato
        // Anders 2 dagen na dato
        private CalendarDateRange GetDisabledDatesInFuture(bool reservationIsForCompetition, bool loggedInUserIsCoach)
        {
            var now = DateTime.Now;
            var maxDate = DateTime.MaxValue;
            if (reservationIsForCompetition) return new CalendarDateRange(now.AddYears(1).AddDays(1), maxDate);
            else if (loggedInUserIsCoach) return new CalendarDateRange(now.AddDays(8), maxDate);
            else return new CalendarDateRange(now.AddDays(3), maxDate);
        }
    }
}