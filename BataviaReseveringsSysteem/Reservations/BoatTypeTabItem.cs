using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Models;
using Views;
using Controllers;
using ScreenSwitcher;
using BataviaReseveringsSysteemDatabase;

namespace Reserve
{
    public class BoatTypeTabItem : TabItem
    {
        public Grid Grid { get; set; }
        public Calendar Calendar { get; set; }
        public PlannerGrid PlannerGrid { get; set; }
        public List<Reservation> Reservations { get; set; }
        public BoatView BoatView { get; set; }
        public int User { get; set; }
        public List<Boat> Boats { get; set; }
        public Boat.BoatType BoatType { get; set; }
        public ComboBox ReservationDurationComboBox = new ComboBox();
        public ComboBox BoatNamesComboBox = new ComboBox();
        private ComboBox _reservationStartComboBox;
        private Button OkButton;
        private const int AmountOfAvailableQuarters = 8;

        public BoatTypeTabItem(List<Boat> boats, Boat.BoatType type, List<Reservation> reservations)
        {
            BoatView = new BoatView();
            BoatType = type;
            Header = type.ToString();
            Reservations = reservations;
            Grid = new Grid();
            MakeRegisterBtnVisibleAfterChoice();
            FillComboNames();
            Calendar = MakeCalendar();
            Grid.Children.Add(Calendar);
            Grid.Children.Add(new Label
            {
                Content = "Boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(300, 450, 0, 0),
                FontSize = 26
            });
            Grid.Children.Add(new Label
            {
                Content = "Eigenschappen boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(300, 150, 0, 0),
                FontSize = 26
            });

            Grid.Children.Add(BoatView);
            Grid.Children.Add(new Label
            {
                Content = "Duur reservering:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(580, 340, 0, 0),
                FontSize = 26
            });
            var annulerenButton = new Button
            {
                Content = "Annuleren",
                Width = 120,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(290, 784, 0, 0),
            };
            annulerenButton.Click += (sender, e) => Switcher.Switch(new Dashboard());
            Grid.Children.Add(annulerenButton);
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;
            var claimedSlotsForThisDay = GetClaimedSlotsForThisDayAndBoat(DateTime.Now, selectedBoatString);
            BoatView.UpdateView(new Boatcontroller().GetBoatWithName(selectedBoatString));
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlotsForThisDay);
            Grid.Children.Add(PlannerGrid);
            Grid.Children.Add(new Label
            {
                Content = "Starttijd:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(300, 340, 0, 0),
                FontSize = 26
            });
            _reservationStartComboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 245,
                Margin = new Thickness(310, 380, 0, 0),
                SelectedIndex = 0,
                FontSize = 26
                
            };
            _reservationStartComboBox.DropDownClosed += OnStartComboBoxClick;
            PopulateStartTimeComboBox(DateTime.Now, earliestSlot, latestSlot, claimedSlotsForThisDay);
            Grid.Children.Add(_reservationStartComboBox);
            Content = Grid;
            FillComboTimes();
            OnStartComboBoxClick(null, null);
            ReservationDurationComboBox.DropDownClosed += OnDurationComboBoxClick;
        }

        // Deze methode vult combobox met bootnamen
        public void FillComboNames()
        {
            BoatNamesComboBox.Name = "ComboBoatName";
            BoatNamesComboBox.Tag =
            BoatNamesComboBox.Width = 245;
            BoatNamesComboBox.Height = 45;
            BoatNamesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
            BoatNamesComboBox.Margin = new Thickness(310, 270, 0, 0);
            BoatNamesComboBox.SelectedIndex = 0;
            BoatNamesComboBox.FontSize = 26;
            BoatNamesComboBox.DropDownClosed += OnBoatNamesComboBoxClicked;
            Grid.Children.Add(BoatNamesComboBox);
            using (var context = new Database())
                foreach (var item in from db in context.Boats where db.Type == BoatType select db.Name)
                    BoatNamesComboBox.Items.Add(item);
        }

        // Deze methode vult de combobox met tijden van 15 min tot 2 uur
        public void FillComboTimes()
        {
            ReservationDurationComboBox.Name = "ComboTimes";
            ReservationDurationComboBox.Width = 120;
            ReservationDurationComboBox.Height = 25;
            ReservationDurationComboBox.Width = 245;
            ReservationDurationComboBox.Height = 45;
            ReservationDurationComboBox.FontSize = 26;
            ReservationDurationComboBox.HorizontalAlignment = HorizontalAlignment.Left;
            ReservationDurationComboBox.Margin = new Thickness(600, 37, 0, 0);
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

        // De methode wanneer je een tijdsduur voor afschrijving kiest
        private void OnDurationComboBoxClick(object sender, EventArgs e)
        {
            var selectedDate = Calendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlots = GetClaimedSlotsForThisDayAndBoat(selectedDateValue, (string)BoatNamesComboBox.SelectedValue);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlots, latestSlot);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlots, aboutToBeClaimedSlots);
        }

        // De methode voor wanneer je een starttijd kiest
        private void OnStartComboBoxClick(object sender, EventArgs eventArgs)
        {
            var selectedDate = Calendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var claimedSlots = GetClaimedSlotsForThisDayAndBoat(selectedDateValue, (string)BoatNamesComboBox.SelectedValue);
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
            var endSlot = GenerateEndTime(startSlot);
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = DateTimeToDayQuarter(endSlot);
            return endSlotDayQuarter - startSlotDayQuarter;
        }

        // Deze methode
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
            var aboutToBeClaimedSlots = new List<DateTime>();
            var startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
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

        // Deze methode maakt de "afschrijven" button klikbaar 
        public void MakeRegisterBtnVisibleAfterChoice()
        {
            OkButton = new Button { Name = "okBtn", FontSize = 26, Content = "Afschrijven", Width = 245, Height = 45, IsEnabled = false };
            OkButton.Click += OkBtn_Click;
            OkButton.HorizontalAlignment = HorizontalAlignment.Left;
            OkButton.Margin = new Thickness(150, 620, 0, 0);
            Grid.Children.Add(OkButton);
        }

        // Deze methode zal de lengte van afschrijfperiode nemen die gekozen is in de combobox
        public DateTime GenerateEndTime(DateTime startTime) => startTime
            .AddHours(int.Parse(ReservationDurationComboBox.SelectedValue.ToString()[1].ToString()))
            .AddMinutes(int.Parse(ReservationDurationComboBox.SelectedValue.ToString().Substring(3)));


        // Zodra de OkBtn is aangeklikt zal de boot worden afgeschreven na messagebox dialoog bevestiging
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Wilt u uw afschrijving definitief maken?",
                    "Afschrijving bevestigen",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) !=
                MessageBoxResult.Yes)
                return;
            using (var context = new Database())
            {
                var boat = (from db in context.Boats
                            where db.Name.Equals((string)BoatNamesComboBox.SelectedValue)
                            select db).First();
                User = LoginView.LoggedMember;
                var startTime = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                var startDate = Calendar.SelectedDate.Value.Date;
                var startDateTime = startDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
                var endTime = GenerateEndTime(startDateTime);
                var newReservation = new Reservation(boat, User, startDateTime, endTime);
                if (MessageBox.Show("De boot is succesvol afgeschreven",
                        "Melding",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information) !=
                    MessageBoxResult.OK)
                    return;
                var selectedDate = Calendar.SelectedDate;
                if (!selectedDate.HasValue) return;
                var selectedDateValue = selectedDate.Value;
                var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
                var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
                var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
                var selectedBoat = (string)BoatNamesComboBox.SelectedValue;
                var claimedSlotsForThisDayAndBoat = GetClaimedSlotsForThisDayAndBoat(selectedDateValue, selectedBoat);
                PopulateStartTimeComboBox(selectedDateValue, earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat);
                var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlotsForThisDayAndBoat, latestSlot);
                var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
                var amountOfSlotsToBeClaimed =
                    GetAmountOfSlotsToBeClaimed(amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed);
                var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
                PopulateDurationTimeComboBox(amountOfClaimableSlots);
                PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
                context.Reservations.Add(newReservation);
                context.SaveChanges();
                Switcher.Switch(new Dashboard());
            }
        }


        // Deze methode zorgt voor de afhandeling van boot keuze in combobox
        private void OnBoatNamesComboBoxClicked(object sender, EventArgs e)
        {
            var selectedDate = Calendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;
            var selectedBoat = new Boatcontroller().GetBoatWithName(selectedBoatString);
            BoatView.UpdateView(selectedBoat);
            var claimedSlotsForThisDayAndBoat = GetClaimedSlotsForThisDayAndBoat(selectedDateValue, selectedBoatString);
            PopulateStartTimeComboBox(selectedDateValue, earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlotsForThisDayAndBoat, latestSlot);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed =
                GetAmountOfSlotsToBeClaimed(amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed);
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
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
            var selectedBoat = (string)BoatNamesComboBox.SelectedValue;
            var claimedSlotsForThisDayAndBoat = GetClaimedSlotsForThisDayAndBoat(selectedDateValue, selectedBoat);
            PopulateStartTimeComboBox(selectedDateValue, earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat);
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlotsForThisDayAndBoat, latestSlot);
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed =
                GetAmountOfSlotsToBeClaimed(amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed);
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
        }

        private int GetAmountOfSlotsToBeClaimed(int amountOfClaimableSlots, int amountOfSlotsWantingToBeClaimed) =>
            new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();

        private List<DateTime> GetClaimedSlotsForThisDayAndBoat(DateTime selectedDate, string selectedBoatString)
        {
            var claimedSlots = new List<DateTime>();
            var selectedBoat = new Boatcontroller().GetBoatWithName(selectedBoatString);
            var reservations = new ReservationController().GetReservationsForDayAndBoatThatAreNotDeleted(selectedDate, selectedBoat);
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