using System;
using System.Windows;
using System.Windows.Controls;

namespace BataviaReseveringsSysteem.Reservations
{
    // De kalender. Ik heb een aparte klasse gemaakt om zo de BoatTypeTabItem een beetje uit te dunnen
    public class BoatTypeTabItemCalendar : Calendar
    {
        public BoatTypeTabItemCalendar()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            // Selecteer een datum.
            // Als je dit niet doet, kan de SelectedDate niet worden uitgelezen en dat leidt tot een hoop gedoe
            // Deze code zorgt ervoor dat SelectedDate altijd kan worden uitgelezen
            SelectedDate = DateTime.Now;

            // Alles in het verleden is niet meer aanklikbaar
            BlackoutDates.AddDatesInPast();
            
            // Alles na overmorgen is niet meer aanklikbaar
            BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(3), DateTime.MaxValue));
        }
    }
}