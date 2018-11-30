using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Database;
using Controllers;
using Models;
using ScreenSwitcher;
using Views;
using FormatException = System.FormatException;

namespace BataviaReseveringsSysteem.Reservations
{
    public class BoatTypeTabItem : TabItem
    {
        public Grid Grid { get; set; }
        public BoatTypeTabItemCalendar BoatTypeTabItemCalendar { get; set; }
        public PlannerGrid PlannerGrid { get; set; }
        public List<Reservation> Reservations { get; set; }
        public BoatView BoatView { get; set; }
        public List<Boat> Boats { get; set; }
        public Boat.BoatType BoatType { get; set; }
        public ComboBox ReservationDurationComboBox = new ComboBox();
        public ComboBox BoatNamesComboBox = new ComboBox();
        private ComboBox _reservationStartComboBox;
        private Button _okButton;
        private const int AmountOfAvailableQuarters = 8;
        private Label _noSlotsAvailableLabel;

        public BoatTypeTabItem(List<Boat> boats, Boat.BoatType type, List<Reservation> reservations)
        {
            _noSlotsAvailableLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = "Er zijn geen boten meer afschrijfbaar vandaag",
                Visibility = Visibility.Hidden
            };
            BoatView = new BoatView();
            BoatType = type;
            Header = type.ToString();
            Reservations = reservations;
            Grid = new Grid();
            MakeRegisterBtnVisibleAfterChoice();
            FillComboNames();
            BoatTypeTabItemCalendar = new BoatTypeTabItemCalendar();
            BoatTypeTabItemCalendar.SelectedDatesChanged += OnCalendarClicked;
            Grid.Children.Add(BoatTypeTabItemCalendar);
            Grid.Children.Add(new Label
            {
                Content = "Boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 200, 0, 0)
            });
            Grid.Children.Add(new Label
            {
                Content = "Eigenschappen boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(150, 180, 0, 0)
            });

            Grid.Children.Add(BoatView);
            Grid.Children.Add(new Label
            {
                Content = "Duur reservering:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 250, 0, 0)
            });
            var annulerenButton = new Button
            {
                Content = "Annuleren",
                Width = 120,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(290, 284, 0, 0),
            };
            annulerenButton.Click += (sender, e) => Switcher.Switch(new Dashboard());
            Grid.Children.Add(annulerenButton);
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;
            var claimedSlotsForThisDay = GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime.Now, selectedBoatString, earliestSlot, latestSlot);
            BoatView.UpdateView(new Boatcontroller().GetBoatWithName(selectedBoatString));
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
            FillComboTimes();
            OnStartComboBoxClick(null, null);
            ReservationDurationComboBox.DropDownClosed += OnDurationComboBoxClick;
            Grid.Children.Add(_noSlotsAvailableLabel);
        }

        private void OnDurationComboBoxClick(object sender, EventArgs e)
        {
            var selectedDate = BoatTypeTabItemCalendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlots = GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(selectedDateValue,
                (string)BoatNamesComboBox.SelectedValue, earliestSlot, latestSlot);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlots, latestSlot);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlots, aboutToBeClaimedSlots);
        }

        private void OnStartComboBoxClick(object sender, EventArgs eventArgs)
        {
            var selectedDate = BoatTypeTabItemCalendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlots = GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(selectedDateValue,
                (string)BoatNamesComboBox.SelectedValue, earliestSlot, latestSlot);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlots, latestSlot);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlots, aboutToBeClaimedSlots);
        }

        private int GetAmountOfSlotsWantingToBeClaimed()
        {
            DateTime startSlot;
            try
            {
                startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                DateTime.Parse(ReservationDurationComboBox.SelectedValue.ToString());
            }
            catch (FormatException)
            {
                return 0;
            }
            var endSlot = GenerateEndTime(startSlot);
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = DateTimeToDayQuarter(endSlot);
            return endSlotDayQuarter - startSlotDayQuarter;
        }

        private int GetAmountOfClaimableSlots(IEnumerable<DateTime> claimedSlots, DateTime latestSlot)
        {
            int startSlotDayQuarter;
            try
            {
                startSlotDayQuarter = DateTimeToDayQuarter(DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString()));
            }
            catch (FormatException)
            {
                return 0;
            }
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
            if (oldSelectedIndex < 0) oldSelectedIndex = 0;
            itemsInComboBox.Clear();
            var amountOfDurationTimeStrings =
                new[] { amountOfSlotsToNextUnclaimableSlot, AmountOfAvailableQuarters }.Min();
            GenerateClaimableDurationStrings(amountOfDurationTimeStrings).ForEach(claimableDurationString =>
                itemsInComboBox.Add(claimableDurationString));
            var amountOfItems = itemsInComboBox.Count;
            if (amountOfItems == 0)
            {
                ReservationDurationComboBox.SelectedValue = "(geen slots meer reserveerbaar vandaag)";
            }
            else
            {
                var highestSelectableIndex = amountOfItems - 1;
                ReservationDurationComboBox.SelectedIndex = new[] { oldSelectedIndex, highestSelectableIndex }.Min();
            }
        }

        private List<string> GenerateClaimableDurationStrings(int amountOfSlotsToBeClaimed)
        {
            var claimableDurationStrings = new List<string>();
            for (var i = 1; i <= amountOfSlotsToBeClaimed; i++)
            {
                var slotString = $"0{i / 4}:{i % 4 * 15}";
                if (i % 4 == 0) slotString += "0";
                claimableDurationStrings.Add(slotString);
            }
            return claimableDurationStrings;
        }

        private List<DateTime> GetAboutToBeClaimedSlots(DateTime displayedDate, int amountOfSlotsToBeClaimed)
        {
            DateTime startSlot;
            try
            {
                startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
            }
            catch (FormatException)
            {
                return new List<DateTime>();
            }
            var aboutToBeClaimedSlots = new List<DateTime>();
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = startSlotDayQuarter + amountOfSlotsToBeClaimed;
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
            if (_reservationStartComboBox.Items.Count == 0)
            {
                _reservationStartComboBox.SelectedValue = "(geen slots beschikbaar)";
                _noSlotsAvailableLabel.Visibility = Visibility.Visible;
                _okButton.IsEnabled = false;
            }
            else
            {
                _reservationStartComboBox.SelectedIndex = 0;
                _noSlotsAvailableLabel.Visibility = Visibility.Hidden;
                _okButton.IsEnabled = true;
            }
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

        private DateTime GetEarliestSlot(DateTime sunrise) => TopRoundTimeToSlot(sunrise);

        private DateTime TopRoundTimeToSlot(DateTime time) => time.AddMinutes(15 - time.Minute % 15);

        private DateTime GetLatestSlot(DateTime sunset) => BottomRoundTimeToSlot(sunset);

        private DateTime BottomRoundTimeToSlot(DateTime time) => time.AddMinutes(-(time.Minute % 15));

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
            _okButton = new Button { Name = "okBtn", Content = "Afschrijven", Width = 120, Height = 25, IsEnabled = false };
            _okButton.Click += OkBtn_Click;
            _okButton.HorizontalAlignment = HorizontalAlignment.Left;
            _okButton.Margin = new Thickness(150, 120, 0, 0);
            Grid.Children.Add(_okButton);
        }

        // This method will take the length of the reserve period from the selected comboboxItem
        public DateTime GenerateEndTime(DateTime startTime) => startTime
            .AddHours(int.Parse(ReservationDurationComboBox.SelectedValue.ToString()[1].ToString()))
            .AddMinutes(int.Parse(ReservationDurationComboBox.SelectedValue.ToString().Substring(3)));


        // When the button is clicked, boats will be reserved after messagebox dialog comfirmation
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wilt u uw afschrijving definitief maken?",
                    "Afschrijving bevestigen",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) !=
                MessageBoxResult.Yes)
                return;
            using (var context = new DataBase())
            {
                var boat = (from db in context.Boats
                            where db.Name.Equals((string)BoatNamesComboBox.SelectedValue)
                            select db).First();
                var startTime = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                var endTime = GenerateEndTime(startTime);
                // TODO: new Member() veranderen naar member die deze afschrijving maakt.
                var rs1 = new Reservation(boat, LoginView.User, startTime, endTime);
                context.Reservations.Add(rs1);
                context.SaveChanges();
            }
            if (MessageBox.Show("De boot is succesvol afgeschreven",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information) !=
                MessageBoxResult.OK)
                return;
            Switcher.Switch(new Dashboard());
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
            BoatNamesComboBox.DropDownClosed += OnBoatNamesComboBoxClicked;
            Grid.Children.Add(BoatNamesComboBox);
            using (var context = new DataBase())
                foreach (var item in from db in context.Boats where db.Type == BoatType select db.Name)
                    BoatNamesComboBox.Items.Add(item);
        }

        private void OnBoatNamesComboBoxClicked(object sender, EventArgs e)
        {
            FullRefresh();
            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;
            var selectedBoat = new Boatcontroller().GetBoatWithName(selectedBoatString);
            BoatView.UpdateView(selectedBoat);
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

        private void OnCalendarClicked(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => FullRefresh();

        private void FullRefresh()
        {
            var selectedDate = BoatTypeTabItemCalendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var selectedBoat = (string)BoatNamesComboBox.SelectedValue;
            var claimedSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(selectedDateValue, selectedBoat, earliestSlot, latestSlot);
            PopulateStartTimeComboBox(selectedDateValue, earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlotsForThisDayAndBoat, latestSlot);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed =
                GetAmountOfSlotsToBeClaimed(amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed);
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
        }

        private int GetAmountOfSlotsToBeClaimed(int amountOfClaimableSlots, int amountOfSlotsWantingToBeClaimed) =>
            new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();

        private List<DateTime> GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime selectedDate, string selectedBoatString,
            DateTime earliestSlot, DateTime latestSlot)
        {
            var claimedSlots = new List<DateTime>();
            var selectedBoat = new Boatcontroller().GetBoatWithName(selectedBoatString);
            var reservations = new ReservationController().GetReservationsForDayAndBoat(selectedDate, selectedBoat);
            var now = DateTime.Now;
            reservations.ForEach(reservation =>
            {
                var endQuarter = DateTimeToDayQuarter(reservation.End);
                for (var i = DateTimeToDayQuarter(reservation.Start); i < endQuarter; i++)
                    claimedSlots.Add(DayQuarterToDateTime(selectedDate, i));
            });
            var datePartOfSelectedDate = selectedDate.Date;
            var datePartOfDateTimeNow = now.Date;
            if (datePartOfSelectedDate.Equals(datePartOfDateTimeNow))
            {
                return GetClaimedAndPastSlots(claimedSlots, now, earliestSlot, latestSlot);
            }
            var twoDaysFromNow = now.AddDays(2);
            var datePartOfTwoDaysFromNow = twoDaysFromNow.Date;
            if (datePartOfSelectedDate.Equals(datePartOfTwoDaysFromNow))
                return GetClaimedAndTooDistantSlots(claimedSlots, twoDaysFromNow, earliestSlot, latestSlot);
            return claimedSlots;
        }

        private List<DateTime> GetClaimedAndPastSlots(List<DateTime> claimedSlots, DateTime now, DateTime earliestSlot,
            DateTime latestSlot)
        {
            var claimedAndPastSlots = new List<DateTime>(claimedSlots);
            var earliestSlotDayQuarter = DateTimeToDayQuarter(earliestSlot);
            var slotThatIsNow = DateTimeToDayQuarter(TopRoundTimeToSlot(now));
            var latestSlotDayQuarter = DateTimeToDayQuarter(latestSlot);
            var latestSlotThatShouldBeMarkedAsDue = new[] { slotThatIsNow, latestSlotDayQuarter }.Min();
            for (var i = earliestSlotDayQuarter; i < latestSlotThatShouldBeMarkedAsDue; i++)
            {
                var passedSlot = DayQuarterToDateTime(now, i);
                if (!claimedSlots.Contains(passedSlot)) claimedAndPastSlots.Add(passedSlot);
            }

            return claimedAndPastSlots;
        }

        private List<DateTime> GetClaimedAndTooDistantSlots(List<DateTime> claimedSlots, DateTime twoDaysFromNow,
            DateTime earliestSlot, DateTime latestSlot)
        {
            var claimedAndTooDistantSlots = new List<DateTime>(claimedSlots);
            var earliestSlotDayQuarter = DateTimeToDayQuarter(earliestSlot);
            var slotThatIsTwoDaysFromNow = DateTimeToDayQuarter(BottomRoundTimeToSlot(twoDaysFromNow));
            var earliestSlotThatShouldBeMarkedAsDue =
                new[] { earliestSlotDayQuarter, slotThatIsTwoDaysFromNow }.Max();
            var latestSlotDayQuarter = DateTimeToDayQuarter(latestSlot);
            for (var i = earliestSlotThatShouldBeMarkedAsDue; i < latestSlotDayQuarter; i++)
            {
                var tooDistantSlot = DayQuarterToDateTime(twoDaysFromNow, i);
                if (!claimedSlots.Contains(tooDistantSlot)) claimedAndTooDistantSlots.Add(tooDistantSlot);
            }

            return claimedAndTooDistantSlots;
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