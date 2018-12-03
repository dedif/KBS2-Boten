using System;
using System.Windows;
using System.Windows.Controls;

namespace BataviaReseveringsSysteem.Reservations
{
    public class BoatTypeTabItemCalendar : Calendar
    {
        public BoatTypeTabItemCalendar()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            SelectedDate = DateTime.Now;
            BlackoutDates.AddDatesInPast();
            BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(3), DateTime.MaxValue));
        }
    }
}