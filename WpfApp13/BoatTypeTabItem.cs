﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using ConsoleApp1;
using WpfApp13;

namespace WpfApp6
{
    public class BoatTypeTabItem : TabItem
    {
        public Grid Grid { get; set; }
        public Calendar Calendar { get; set; }
        public PlannerGrid PlannerGrid { get; set; }
        public List<Reservation> Reservations { get; set; }
        public BoatView BoatView { get; set; }
        public List<Boat> Boats { get; set; }
        public Boat.BoatType BoatType { get; set; }
        public ComboBox ReservationDurationComboBox = new ComboBox();
        public ComboBox BoatNamesComboBox = new ComboBox();
        private ComboBox _reservationStartComboBox;
        private Button OkButton;
        private const int AmountOfAvailableQuarters = 8;

        public BoatTypeTabItem(List<Boat> boats, Boat.BoatType type, List<Reservation> reservations)
        {
            BoatType = type;
            Header = type.ToString();
            Reservations = reservations;
            Grid = new Grid();
            MakeRegisterBtnVisibleAfterChoice();
            Calendar = MakeCalendar();
            Grid.Children.Add(Calendar);
            Grid.Children.Add(new Label
            {
                Content = "Boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 180, 0, 0)
            });
            Grid.Children.Add(new Label
            {
                Content = "Eigenschappen boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(150, 180, 0, 0)
            });
            Grid.Children.Add(new BoatView());
            Grid.Children.Add(new Label
            {
                Content = "Duur reservering:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 250, 0, 0)
            });
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlotsForThisDay = GetClaimedSlotsForThisDay(DateTime.Now);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlotsForThisDay);
            Grid.Children.Add(PlannerGrid);
            Grid.Children.Add(new Label
            {
                Content = "Starttijd:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 310, 0, 0)
            });
            _reservationStartComboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 340, 0, 0),
                SelectedIndex = 0
            };
            _reservationStartComboBox.DropDownClosed += OnStartComboBoxClick;
            PopulateStartTimeComboBox(DateTime.Now, earliestSlot, latestSlot, claimedSlotsForThisDay);
            Grid.Children.Add(_reservationStartComboBox);
            Content = Grid;
            FillComboNames();
            FillComboTimes();
            OnStartComboBoxClick(null, null);
            ReservationDurationComboBox.DropDownClosed += OnDurationComboBoxClick;
        }

        private void OnDurationComboBoxClick(object sender, EventArgs e)
        {
            var selectedDate = Calendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlots = GetClaimedSlotsForThisDay(selectedDateValue);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlots, latestSlot);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlots, aboutToBeClaimedSlots);
        }

        private void OnStartComboBoxClick(object sender, EventArgs eventArgs)
        {
            var selectedDate = Calendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlots = GetClaimedSlotsForThisDay(selectedDateValue);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlots, latestSlot);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlots, aboutToBeClaimedSlots);
        }

        private int GetAmountOfSlotsWantingToBeClaimed()
        {
            var startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
            var endSlot = startSlot
                .AddHours(int.Parse(ReservationDurationComboBox.SelectedValue.ToString()[1].ToString()))
                .AddMinutes(int.Parse(ReservationDurationComboBox.SelectedValue.ToString().Substring(3)));
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = DateTimeToDayQuarter(endSlot);
            return endSlotDayQuarter - startSlotDayQuarter;
        }

        private int GetAmountOfClaimableSlots(IEnumerable<DateTime> claimedSlots, DateTime latestSlot)
        {
            var startSlotDayQuarter = DateTimeToDayQuarter(DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString()));
            var endSlotDayQuarter = DateTimeToDayQuarter(latestSlot);
            foreach (var claimedSlot in claimedSlots)
            {
                var claimedSlotDayQuarter = DateTimeToDayQuarter(claimedSlot);
                if (claimedSlotDayQuarter < startSlotDayQuarter) continue;
                endSlotDayQuarter = claimedSlotDayQuarter;
                break;
            }

            return endSlotDayQuarter - startSlotDayQuarter;
        }

        private void PopulateDurationTimeComboBox(int amountOfSlotsToNextUnclaimableSlot)
        {
            var itemsInComboBox = ReservationDurationComboBox.Items;
            var oldSelectedIndex = ReservationDurationComboBox.SelectedIndex;
            itemsInComboBox.Clear();
            var amountOfDurationTimeStrings =
                new[] { amountOfSlotsToNextUnclaimableSlot, AmountOfAvailableQuarters }.Min();
            GenerateClaimableDurationStrings(amountOfDurationTimeStrings).ForEach(claimableDurationString =>
                itemsInComboBox.Add(claimableDurationString));
            var amountOfItems = itemsInComboBox.Count;
            var highestSelectableIndex = amountOfItems - 1;
            ReservationDurationComboBox.SelectedIndex = new[] { oldSelectedIndex, highestSelectableIndex }.Min();
            Console.WriteLine(ReservationDurationComboBox.SelectedValue);
        }

        private List<string> GenerateClaimableDurationStrings(int amountOfSlotsToBeClaimed)
        {
            var claimableDurationStrings = new List<string>();
            for (int i = 1; i <= amountOfSlotsToBeClaimed; i++)
            {
                var slotString = $"0{i / 4}:{i % 4 * 15}";
                if (i % 4 == 0) slotString += "0";
                claimableDurationStrings.Add(slotString);
            }

            return claimableDurationStrings;
        }

        private List<DateTime> GetAboutToBeClaimedSlots(DateTime displayedDate, int amountOfSlotsToBeClaimed)
        {
            var aboutToBeClaimedSlots = new List<DateTime>();
            var startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = startSlotDayQuarter + amountOfSlotsToBeClaimed;
            //            var endSlot = startSlot
            //                .AddHours(int.Parse(ReservationDurationComboBox.SelectedValue.ToString()[1].ToString()))
            //                .AddMinutes(int.Parse(ReservationDurationComboBox.SelectedValue.ToString().Substring(3)));
            //            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            //            var endSlotDayQuarter = DateTimeToDayQuarter(endSlot);
            for (var i = startSlotDayQuarter; i < endSlotDayQuarter; i++)
                aboutToBeClaimedSlots.Add(DayQuarterToDateTime(displayedDate, i));

            return aboutToBeClaimedSlots;
        }

        private void PopulateStartTimeComboBox(DateTime selectedDate, DateTime earliestSlot, DateTime latestSlot,
            List<DateTime> claimedSlotsForThisDay)
        {
            _reservationStartComboBox.Items.Clear();
            GenerateClaimableStartSlots(selectedDate, earliestSlot, latestSlot, claimedSlotsForThisDay)
                .ForEach(claimableStartSlot => _reservationStartComboBox.Items.Add(claimableStartSlot.ToString("t")));
            _reservationStartComboBox.SelectedIndex = 0;
            OkButton.IsEnabled = true;
        }

        private List<DateTime> GenerateClaimableStartSlots(DateTime selectedDate,
            DateTime earliestSlot,
            DateTime latestSlot,
            List<DateTime> unavailableSlots)
        {
            var claimableSlots = new List<DateTime>();
            var earliestSlotDayQuarter = DateTimeToDayQuarter(earliestSlot);
            var latestSlotDayQuarter = DateTimeToDayQuarter(latestSlot);
            for (var i = earliestSlotDayQuarter; i < latestSlotDayQuarter; i++)
                if (!unavailableSlots.Exists(unavailableSlot => DateTimeToDayQuarter(unavailableSlot) == i))
                    claimableSlots.Add(DayQuarterToDateTime(selectedDate, i));
            return claimableSlots;
        }

        private DateTime GetEarliestSlot(DateTime sunrise) => sunrise.AddMinutes(15 - sunrise.Minute % 15);

        private DateTime GetLatestSlot(DateTime sunset) => sunset.AddMinutes(-(sunset.Minute % 15));

        public void FillComboTimes()
        {
            ReservationDurationComboBox.Name = "ComboTimes";
            ReservationDurationComboBox.Width = 120;
            ReservationDurationComboBox.Height = 25;
            ReservationDurationComboBox.HorizontalAlignment = HorizontalAlignment.Left;
            ReservationDurationComboBox.Margin = new Thickness(10, 120, 0, 0);
            ReservationDurationComboBox.SelectedIndex = 0;
            ReservationDurationComboBox.Items.Add("00:15");
            ReservationDurationComboBox.Items.Add("00:30");
            ReservationDurationComboBox.Items.Add("00:45");
            ReservationDurationComboBox.Items.Add("01:00");
            ReservationDurationComboBox.Items.Add("01:15");
            ReservationDurationComboBox.Items.Add("01:30");
            ReservationDurationComboBox.Items.Add("01:45");
            ReservationDurationComboBox.Items.Add("02:00");
            Grid.Children.Add(ReservationDurationComboBox);

        }

        public void MakeRegisterBtnVisibleAfterChoice()
        {
            OkButton = new Button { Name = "okBtn", Content = "Afschrijven", Width = 120, Height = 25, IsEnabled = false };
            OkButton.Click += OkBtn_Click;
            OkButton.HorizontalAlignment = HorizontalAlignment.Left;
            OkButton.Margin = new Thickness(150, 120, 0, 0);
            Grid.Children.Add(OkButton);
        }

        public int CalculateQuarterFromComboBox()
        {
            int timeEnd;
            var oldTime = ReservationDurationComboBox.Text;

            // if hours is < 1

            if (oldTime == "00:15" || oldTime == "00:30" || oldTime == "00:45")
            {
                char[] myChar = { '0', '0', ':' };
                var newTime = oldTime.TrimStart(myChar);
                timeEnd = int.Parse(newTime);
            }

            // if hours is >= 1 && < 2

            else if (oldTime == "01:00" || oldTime == "01:15" || oldTime == "01:30" || oldTime == "01:45")
            {
                char[] myChar = { '0', '1', ':' };
                var newTime = oldTime.TrimStart(myChar);
                timeEnd = int.Parse(newTime + 60);

            }

            // if hours is >= 2

            else timeEnd = 120;

            return timeEnd;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            var Succes = MessageBox.Show(
                            "Wilt u uw afschrijving definitief maken?",
                            "Afschrijving bevestigen",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);
            if (Succes != MessageBoxResult.Yes) return;
            using (var context = new Database())
            {
                var boat = (from db in context.Boats
                    where db.Name.Equals((string) BoatNamesComboBox.SelectedValue)
                    select db).First();
                var startTime = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                var rs1 = new Reservation(boat, new Member(), startTime,
                    startTime.AddMinutes(CalculateQuarterFromComboBox()));
                context.Reservations.Add(rs1);
                context.SaveChanges();
                MessageBox.Show(
                    "De boot is succesvol afgeschreven",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

            }
        }
        public void FillComboNames()
        {
            BoatNamesComboBox.Name = "ComboBoatName";
            BoatNamesComboBox.Tag =
            BoatNamesComboBox.Width = 120;
            BoatNamesComboBox.Height = 25;
            BoatNamesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
            BoatNamesComboBox.Margin = new Thickness(10, 0, 0, 0);
            BoatNamesComboBox.SelectedIndex = 0;
            Grid.Children.Add(BoatNamesComboBox);

            using (var context = new Database())
                foreach (var item in from db in context.Boats where db.Type == BoatType select db.Name)
                    BoatNamesComboBox.Items.Add(item);
        }

        private Calendar MakeCalendar()
        {
            var calendar = new Calendar
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                SelectedDate = DateTime.Now
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
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            PlannerGrid.Populate(earliestSlot, latestSlot, GetClaimedSlotsForThisDay(selectedDateValue));

            PopulateStartTimeComboBox(selectedDateValue, earliestSlot, latestSlot, GetClaimedSlotsForThisDay(selectedDateValue));
        }

        private List<DateTime> GetClaimedSlotsForThisDay(DateTime selectedDate)
        {
            var claimedSlots = new List<DateTime>();
            var reservations = new ReservationController().GetReservationsForDay(selectedDate);
            reservations.ForEach(reservation =>
            {
                var endQuarter = DateTimeToDayQuarter(reservation.End);
                for (var i = DateTimeToDayQuarter(reservation.Start); i < endQuarter; i++)
                    claimedSlots.Add(DayQuarterToDateTime(selectedDate, i));
            });
            return claimedSlots;
        }

        private int DateTimeToDayQuarter(DateTime time) => time.Hour * 4 + time.Minute / 15;

        private DateTime DayQuarterToDateTime(DateTime selectedDate,
            int dayQuarter) => new DateTime(selectedDate.Year,
            selectedDate.Month,
            selectedDate.Day,
            dayQuarter / 4,
            dayQuarter % 4 * 15,
            00);
    }
}