using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;

namespace WpfApp6
{
    public class BoatTypeTabItem : TabItem
    {
        public Grid Grid { get; set; }
        public Calendar Calendar { get; set; }
        public PlannerGrid PlannerGrid { get; set; }
        public List<Reservation> Reservations { get; set; }

        public BoatTypeTabItem(Boat.BoatType type, List<Reservation> reservations)
        {
            Reservations = reservations;
            Grid = new Grid();
            Calendar = MakeCalendar();
            Grid.Children.Add(Calendar);
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            PlannerGrid.Populate(sunriseAndSunsetTimes);
            Grid.Children.Add(PlannerGrid);
            Content = Grid;
        }

        private Calendar MakeCalendar()
        {
            Calendar calendar = new Calendar()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            calendar.SelectedDatesChanged += OnCalendarClicked;
            return calendar;
        }

        private DateTime[] GetSunriseAndSunsetTimes(DateTime selectedDate)
        {
            bool isSunrise = false;
            bool isSunset = false;
            DateTime sunrise = DateTime.Now;
            DateTime sunset = DateTime.Now;
            SunTimes.Instance.CalculateSunRiseSetTimes(
                new SunTimes.LatitudeCoords(
                    52,
                    31,
                    0,
                    SunTimes.LatitudeCoords.Direction.North),
                new SunTimes.LongitudeCoords(
                    6,
                    4,
                    58,
                    SunTimes.LongitudeCoords.Direction.East),
                selectedDate,
                ref sunrise,
                ref sunset,
                ref isSunrise,
                ref isSunset
                );
            return new[] { sunrise, sunset };
        }

        private void OnCalendarClicked(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var selectedDate = Calendar.SelectedDate;
            if (selectedDate.HasValue) PlannerGrid.Populate(GetSunriseAndSunsetTimes(selectedDate.Value));
        }
    }
}