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
    // Het scherm om boten af te schrijven bestaat (nu nog) uit meerdere BoatTypeTabItems.
    // Dit zijn tabjes.
    // Alles (kalender, tijdsslotgrid, knoppen) wordt op deze tabjes geschreven en niet op de tabcontrol op het reserveringsscherm
    public class BoatTypeTabItem : TabItem
    {
        // Het grid waar alles op wordt geplaatst
        public Grid Grid { get; set; }

        // De kalender
        public BoatTypeTabItemCalendar BoatTypeTabItemCalendar { get; set; }

        // Het tabelletje om de tijdsslots aan te klikken
        public PlannerGrid PlannerGrid { get; set; }
        public List<Reservation> Reservations { get; set; }

        // Een statusdisplay van de boten
        public BoatView BoatView { get; set; }
        public int User { get; set; }
        public List<Boat> Boats { get; set; }

        // Aangezien een BoatTypeTabItem een TabItem is waarop je alle boten van een bepaald type kan weergeven, is het handig dat het BoatType erin staat
        public Boat.BoatType BoatType { get; set; }

        // Dropdown voor de tijdsduur van je afschrijving
        public ComboBox ReservationDurationComboBox = new ComboBox();
        public ComboBox BoatNamesComboBox = new ComboBox();

        // Dropdown voor de eerste tijdsslot (dus de start) van je afschrijving
        private ComboBox _reservationStartComboBox;
        private Button _okButton;

        // Je mag maximaal 2 uur lang = 8 kwartier reserveren. In deze variabele staat opgenomen hoeveel kwartieren je mag reserveren (8 dus)
        private const int AmountOfAvailableQuarters = 8;

        // Als het te laat is om vandaag nog te afschrijven,
        // of als het nog donker is, waardoor je geen enkele slot van overmorgen mag afschrijven,
        // dan wordt dit label met de melding dat er geen slots beschikbaar zijn, zichtbaar
        private Label _noSlotsAvailableLabel;

        public BoatTypeTabItem(Boat.BoatType type, List<Reservation> reservations)
        {
            _noSlotsAvailableLabel = new Label
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

            // Maak de grid waar alles in komt
            Grid = new Grid();

            // Maak de "Afschrijven"-knop
            MakeRegisterBtn();

            // Vul de combobox met namen
            FillComboNames();

            BoatTypeTabItemCalendar = new BoatTypeTabItemCalendar();

            // Als er een andere datum wordt geselecteerd, roep dan OnCalendarClicked() aan
            BoatTypeTabItemCalendar.SelectedDatesChanged += OnCalendarClicked;
            Grid.Children.Add(BoatTypeTabItemCalendar);

            // Dit label staat boven de bootnaamcombobox en fungeert als een kopje
            Grid.Children.Add(new Label
            {
                Content = "Boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(300, 450, 0, 0),
                FontSize = 26
            });

            // Dit label staat boven de boatview en fungeert als een kopje
            Grid.Children.Add(new Label
            {
                Content = "Eigenschappen boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(300, 150, 0, 0),
                FontSize = 26
            });

            // De boatview is een paar regels terug al aangemaakt.
            Grid.Children.Add(BoatView);

            // Dit label staat boven de reservationdurationcombobox en fungeert als een kopje
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

            // Als je op "Annuleren" klikt, dan ga je weer terug naar het dashboard
            annulerenButton.Click += (sender, e) => Switcher.Switch(new Dashboard());
            Grid.Children.Add(annulerenButton);

            PlannerGrid = new PlannerGrid();

            // Nu worden de plannergrid, de dropdowns, de kalender en de boatview geïnitialiseerd.
            // Als je dit niet zou doen,
            // dan zouden al deze dingen leeg zijn als je het reserveringsscherm net opent en nog niets hebt aangeklikt.
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);

            // De eerste slot waar het licht is. Komt de zon op om 6.24, dan is de eerste slot van 6.30 tot 6.45
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);

            // De laatste slot waar het licht is. Gaat de zon onder om 20.08, dan is de laatste slot van 19.45 tot 20.00
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);

            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;

            // Haal alle slots op
            // 1) waarbij de geselecteerde boot (we openen het reserveringsscherm net, dus de eerste boot in de botencombobox is de geselecteerde boot) al afgeschreven is
            // 2) al voorbij zijn (maar waar de zon al wel op was)
            // 3) te ver in de toekomst zijn om geclaimd te kunnen worden (maar waar de zon nog niet onder is)
            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime.Now, selectedBoatString, earliestSlot,
                    latestSlot);

            // Weergeef de eigenschappen van de geselecteerde boot (wederom de eerste in de botencombobox)
            BoatView.UpdateView(new Boatcontroller().GetBoatWithName(selectedBoatString));

            // Vul de PlannerGrid met de slots voor deze dag.
            // Alles vòòr de eerste en na de laatste slot wordt donkerblauw.
            // Alle slots die al geclaimd zijn, voorbij zijn of nog niet gereserveerd mogen worden, worden grijs
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat);
            Grid.Children.Add(PlannerGrid);

            // Staat boven de reservationstartcombobox. Fungeert als kopje.
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

            // DropDownClosed wordt getriggerd bij inklappen van de grid.
            _reservationStartComboBox.DropDownClosed += OnStartComboBoxClick;

            // Vul de reservationstartcombobox met de beschikbare starttijden voor een afschrijving.
            // Om een starttijd te mogen kiezen, moet de slot van de starttijd aan de volgende voorwaarden voldoen:
            // 1) Het slot moet na zonsopgang zijn
            // 2) Het slot moet vòòr zonsondergang zijn
            // 3) Het slot is nog niet afgeschreven
            // 4) Het slot begint niet in het verleden
            PopulateStartTimeComboBox(DateTime.Now, earliestSlot, latestSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat);
            Grid.Children.Add(_reservationStartComboBox);

            // Weergeef de grid
            Content = Grid;

            // Kijk of je wel acht kwartier mag afschrijven of dat er afschrijving binnen die acht kwartier geclaimd is.
            OnStartComboBoxClick(null, null);

            ReservationDurationComboBox.DropDownClosed += OnDurationComboBoxClick;

            Grid.Children.Add(_noSlotsAvailableLabel);
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
            using (var context = new DataBase())
                foreach (var item in from db in context.Boats where db.Type == BoatType select db.Name)
                    BoatNamesComboBox.Items.Add(item);
        }

        // Initialiseer de afschrijvingsduur-combobox met acht opties (één tot acht kwartier na de starttijd)
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
            // Als er op de afschrijvingslengte-combobox wordt geklikt, dan wordt de plannergrid opnieuw gerenderd. 

            // Genereer de eerste en de laatste slots die je mag afschrijven,
            var selectedDate = BoatTypeTabItemCalendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);

            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);

            // Genereer de slots die al voorbij zijn, te ver weg zijn of al geclaimd zijn
            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(selectedDateValue,
                    (string)BoatNamesComboBox.SelectedValue, earliestSlot, latestSlot);

            // Genereer de slots die je wil claimen
            // (wordt bekeken aan de hand van de start- en de afschrijvingsduur-comboboxes
            //            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedPastAndTooDistantSlotsForThisDayAndBoat, latestSlot); // TODO: uitzoeken of dit nog nodig is
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            //            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min(); // TODO: uitzoeken of dit nog nodig is
            //            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsWantingToBeClaimed);

            // PlannerGrid opnieuw renderen
            PlannerGrid.Populate(earliestSlot, latestSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
        }

        // De methode voor wanneer je een starttijd kiest
        private void OnStartComboBoxClick(object sender, EventArgs eventArgs)
        {
            // Als de startcombobox wordt gesloten, refresh dan de afschrijvingslengte-combobox en de plannergrid

            // Genereer de eerste en de laatste slots die je mag afschrijven,
            var selectedDate = BoatTypeTabItemCalendar.SelectedDate;
            if (!selectedDate.HasValue) return;
            var selectedDateValue = selectedDate.Value;
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);

            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);

            // Genereer de slots die al voorbij zijn, te ver weg zijn of al geclaimd zijn
            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(selectedDateValue,
                    (string)BoatNamesComboBox.SelectedValue, earliestSlot, latestSlot);

            // De afschrijvingsduur-combobox moet worden ververst.
            // Als er om 13 uur een reservering staat en je hebt eerst 11 uur aangeklikt als start en nu 12 uur,
            // dan mag je ineens 1 uur minder reserveren.
            // Hier wordt dat doorgevoerd
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedPastAndTooDistantSlotsForThisDayAndBoat, latestSlot);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);

            // Zojuist is het aantal geselecteerde slots misschien gewijzigd.
            // Daarom is het belangrijk dat eerst de afschrijvingslengte-combobox wordt bijgewerkt
            // en dan pas gekeken welke slots er geclaimd worden
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            //            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsWantingToBeClaimed);
            //            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);

            PlannerGrid.Populate(earliestSlot, latestSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
        }

        // Verkrijg het aantal slots dat de gebruiker heeft geselecteerd
        private int GetAmountOfSlotsWantingToBeClaimed()
        {
            DateTime startSlot;
            try
            {
                // Als er op deze dag niets afgeschreven mag worden, zijn de startcombobox en de duurcombobox blanco.
                // Ze kunnen dan niet uitgelezen worden
                startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                DateTime.Parse(ReservationDurationComboBox.SelectedValue.ToString());
            }
            catch (FormatException)
            {
                // Als er niets afgeschreven mag worden, dan is het aantal slots dat de gebruiker heeft geselecteerd 0.
                // Er staat dan ook geen groen hokje in de plannergrid
                return 0;
            }
            // Als er wel wat afgeschreven mag worden, dan is het aantal slots de eindslot min de startslot

            // Verkrijg de duur van de slot op basis van de beginslot en de duurcombobox
            var endSlot = GenerateEndTime(startSlot);
            
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = DateTimeToDayQuarter(endSlot);
            return endSlotDayQuarter - startSlotDayQuarter;
        }


        // Kijk hoeveel slots er zitten tussen het geselecteerde slot en de eerstvolgende afgeschreven slot of de eerste slot te ver in de toekomst komt
        private int GetAmountOfClaimableSlots(IEnumerable<DateTime> claimedPastAndTooDistantSlotsForThisDayAndBoat,
            DateTime latestSlot)
        {
            int startSlotDayQuarter;
            try
            {
                // Als er op deze dag niets afgeschreven mag worden, is de startcombobox.
                // Hij kan dan niet uitgelezen worden
                startSlotDayQuarter = DateTimeToDayQuarter(DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString()));
            }
            catch (FormatException)
            {
                // Als er niets afgeschreven mag worden, dan is het aantal slots dat de gebruiker mag afschrijven altijd 0.
                return 0;
            }
            // Zo nee, dan wordt er gescand naar de eerstvolgende afgeschreven slot of de eerste slot die te ver in de toekomst komt

            // Laatst claimbare slot is wordt eerst op de laatste slot voor zonsopgang gezet.
            // Geen grijze slots vandaag? Dan is de laatst claimbare slot de laatste slot voor zonsopgang
            var endSlotDayQuarter = DateTimeToDayQuarter(latestSlot);

            // Ga alle grijze slots langs
            foreach (var claimedPastOrTooDistantSlotForThisDayAndBoat in claimedPastAndTooDistantSlotsForThisDayAndBoat)
            {
                var claimedPastOrTooDistantSlotForThisDayAndBoatDayQuarter =
                    DateTimeToDayQuarter(claimedPastOrTooDistantSlotForThisDayAndBoat);
                if (claimedPastOrTooDistantSlotForThisDayAndBoatDayQuarter < startSlotDayQuarter) continue;
                // Grijs slot gevonden? Dan wordt dat de laatst mogelijke slot
                endSlotDayQuarter = claimedPastOrTooDistantSlotForThisDayAndBoatDayQuarter;
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

        public void MakeRegisterBtn()
        {
            _okButton = new Button { Name = "okBtn", Content = "Afschrijven", Width = 120, Height = 25, IsEnabled = false };
            _okButton.Click += OkBtn_Click;
            _okButton.HorizontalAlignment = HorizontalAlignment.Left;
            _okButton.Margin = new Thickness(150, 120, 0, 0);
            Grid.Children.Add(_okButton);

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
            using (var context = new DataBase())
            {
                var boat = (from db in context.Boats
                            where db.Name.Equals((string)BoatNamesComboBox.SelectedValue)
                            select db).First();
                User = LoginView.UserId;
                var startTime = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                var endTime = GenerateEndTime(startTime);
                // TODO: new Member() veranderen naar member die deze afschrijving maakt.
                var rs1 = new Reservation(boat, startTime, endTime);
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

        // Deze methode zorgt voor de afhandeling van boot keuze in combobox
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