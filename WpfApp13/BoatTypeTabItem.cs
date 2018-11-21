using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ConsoleApp1;

namespace WpfApp6
{
    public class BoatTypeTabItem : TabItem
    {
        public Grid Grid { get; set; }
        public Calendar Calendar { get; set; }
        public PlannerGrid PlannerGrid { get; set; }
        public ComboBox BoatNameComboBox { get; set; }
        public List<Reservation> Reservations { get; set; }
        public BoatView BoatView { get; set; }

        public BoatTypeTabItem(List<Boat> boats, Boat.BoatType type, List<Reservation> reservations)
        {
            Header = type.ToString();
            Reservations = reservations;
            Grid = new Grid();
            Calendar = MakeCalendar();
            Grid.Children.Add(Calendar);
            Grid.Children.Add(new Label
            {
                Content = "Boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 180, 0, 0)
            });
            BoatNameComboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 100,
                Margin = new Thickness(10, 210, 0, 0)
            };
            foreach (var boat in boats) BoatNameComboBox.Items.Add(boat.Name);
            Grid.Children.Add(BoatNameComboBox);
            Grid.Children.Add(new Label
            {
                Content = "Eigenschappen boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(120, 180, 0, 0)
            });
            Grid.Children.Add(new BoatView());
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            PlannerGrid.Populate(sunriseAndSunsetTimes);
            Grid.Children.Add(PlannerGrid);
            Content = Grid;
        }

        private Calendar MakeCalendar()
        {
            var calendar = new Calendar
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            calendar.SelectedDatesChanged += OnCalendarClicked;
            return calendar;
        }

        private DateTime[] GetSunriseAndSunsetTimes(DateTime selectedDate)
        {
            var isSunrise = false;
            var isSunset = false;
            var sunrise = DateTime.Now;
            var sunset = DateTime.Now;
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