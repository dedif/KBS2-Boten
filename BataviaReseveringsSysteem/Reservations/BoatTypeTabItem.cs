using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private bool _selectsDuration;
 
        public BoatTypeTabItem(List<Boat> boats, List<Reservation> reservations)
        {
            _noSlotsAvailableLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Content = "Er zijn geen boten meer afschrijfbaar vandaag",
                Visibility = Visibility.Hidden
            };
 
            BoatView = new BoatView();
 
            Reservations = reservations;
 
            // Maak de grid waar alles in komt
            Grid = new Grid();
 
            // Maak de "Afschrijven"-knop
            MakeRegisterBtn();
 
            // Vul de combobox met namen
            FillComboNames(boats);
 
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
                Margin = new Thickness(580, 290, 0, 0),
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
            PlannerGrid.MouseDown += OnPlannerGridClick;
 
            // Nu worden de plannergrid, de dropdowns, de kalender en de boatview geïnitialiseerd.
            // Als je dit niet zou doen,
            // dan zouden al deze dingen leeg zijn als je het reserveringsscherm net opent en nog niets hebt aangeklikt.
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
 
            // De eerste slot waar het licht is. Komt de zon op om 6.24, dan is de eerste slot van 6.30 tot 6.45
            var earliestSlot = GetFirstLightSlot(sunriseAndSunsetTimes[0]);
 
            // De laatste slot waar het licht is. Gaat de zon onder om 20.08, dan is de laatste slot van 19.45 tot 20.00
            var firstDarknessSlot = GetFirstDarknessSlot(sunriseAndSunsetTimes[1]);
 
            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;
 
            // Haal alle slots op
            // 1) waarbij de geselecteerde boot (we openen het reserveringsscherm net, dus de eerste boot in de botencombobox is de geselecteerde boot) al afgeschreven is
            // 2) al voorbij zijn (maar waar de zon al wel op was)
            // 3) te ver in de toekomst zijn om geclaimd te kunnen worden (maar waar de zon nog niet onder is)
            var now = DateTime.Now;
            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(now, now, selectedBoatString, earliestSlot,
                    firstDarknessSlot);
 
            // Weergeef de eigenschappen van de geselecteerde boot (wederom de eerste in de botencombobox)
            BoatView.UpdateView(new BoatController().GetBoatWithName(selectedBoatString));
 
            // Vul de PlannerGrid met de slots voor deze dag.
            // Alles vòòr de eerste en na de laatste slot wordt donkerblauw.
            // Alle slots die al geclaimd zijn, voorbij zijn of nog niet gereserveerd mogen worden, worden grijs
            PlannerGrid.Populate(earliestSlot, firstDarknessSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat);
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
            PopulateStartTimeComboBox(DateTime.Now, earliestSlot, firstDarknessSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat);
            Grid.Children.Add(_reservationStartComboBox);
 
            // Weergeef de grid
            Content = Grid;
 
            FillComboTimes();
 
            // Kijk of je wel acht kwartier mag afschrijven of dat er afschrijving binnen die acht kwartier geclaimd is.
            OnStartComboBoxClick(null, null);
 
            ReservationDurationComboBox.DropDownClosed += OnDurationComboBoxClick;
 
            Grid.Children.Add(_noSlotsAvailableLabel);
        }
 
        // Vul de bootnaamcombobox
        public void FillComboNames(List<Boat> boats)
        {
            BoatNamesComboBox.Name = "ComboBoatName";
            BoatNamesComboBox.Width = 120;
            BoatNamesComboBox.Height = 25;
            BoatNamesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
            BoatNamesComboBox.Margin = new Thickness(10, 0, 0, 0);
            // De combobox selecteert bij openen van het scherm de eerste boot
            BoatNamesComboBox.SelectedIndex = 0;
            BoatNamesComboBox.DropDownClosed += OnBoatNamesComboBoxClicked;
            Grid.Children.Add(BoatNamesComboBox);
            // Vul de combobox met boten uit de database die corresponderen met dit type
            //            using (var context = new DataBase())
            foreach (var item in boats)
                BoatNamesComboBox.Items.Add(item.Name);
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
            var earliestSlot = GetFirstLightSlot(sunriseAndSunsetTimes[0]);
 
            var firstDarknessSlot = GetFirstDarknessSlot(sunriseAndSunsetTimes[1]);
 
            // Genereer de slots die al voorbij zijn, te ver weg zijn of al geclaimd zijn
            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime.Now, selectedDateValue,
                    (string)BoatNamesComboBox.SelectedValue, earliestSlot, firstDarknessSlot);
 
            // Genereer de slots die je wil claimen
            // (wordt bekeken aan de hand van de start- en de afschrijvingsduur-comboboxes
            //            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedPastAndTooDistantSlotsForThisDayAndBoat, firstDarknessSlot); // TODO: uitzoeken of dit nog nodig is
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            //            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min(); // TODO: uitzoeken of dit nog nodig is
            //            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsWantingToBeClaimed);
 
            // PlannerGrid opnieuw renderen
            PlannerGrid.Populate(earliestSlot, firstDarknessSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
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
            var earliestSlot = GetFirstLightSlot(sunriseAndSunsetTimes[0]);
 
            var firstDarknessSlot = GetFirstDarknessSlot(sunriseAndSunsetTimes[1]);
 
            // Genereer de slots die al voorbij zijn, te ver weg zijn of al geclaimd zijn
            var claimedPastAndTooDistantSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime.Now, selectedDateValue,
                    (string)BoatNamesComboBox.SelectedValue, earliestSlot, firstDarknessSlot);
 
            // De afschrijvingsduur-combobox moet worden ververst.
            // Als er om 13 uur een reservering staat en je hebt eerst 11 uur aangeklikt als start en nu 12 uur,
            // dan mag je ineens 1 uur minder reserveren.
            // Hier wordt dat doorgevoerd
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedPastAndTooDistantSlotsForThisDayAndBoat, firstDarknessSlot);
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
 
            // Zojuist is het aantal geselecteerde slots misschien gewijzigd.
            // Daarom is het belangrijk dat eerst de afschrijvingslengte-combobox wordt bijgewerkt
            // en dan pas gekeken welke slots er geclaimd worden
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            //            var amountOfSlotsToBeClaimed = new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsWantingToBeClaimed);
            //            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
 
            PlannerGrid.Populate(earliestSlot, firstDarknessSlot, claimedPastAndTooDistantSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
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
 
        // Kijk hoeveel slots er zitten tussen het geselecteerde slot en de eerstvolgende afgeschreven slot of de eerste slot die te ver in de toekomst komt
        private int GetAmountOfClaimableSlots(IEnumerable<DateTime> claimedPastAndTooDistantSlotsForThisDayAndBoat,
            DateTime firstDarknessSlot)
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
 
            // Laatst claimbare slot wordt eerst op de laatste slot voor zonsopgang gezet.
            // Geen grijze slots vandaag? Dan is de laatst claimbare slot de laatste slot voor zonsopgang
            var endSlotDayQuarter = DateTimeToDayQuarter(firstDarknessSlot);
 
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
            // Retourneer de hoeveelheid tijd
            return endSlotDayQuarter - startSlotDayQuarter;
        }
 
        // Vul de afschrijvingsduur-combobox
        private void PopulateDurationTimeComboBox(int amountOfSlotsToNextUnclaimableSlot)
        {
            var itemsInComboBox = ReservationDurationComboBox.Items;
            // Verkrijg het item dat nu geselecteerd is
            var oldSelectedIndex = ReservationDurationComboBox.SelectedIndex;
            if (oldSelectedIndex < 0) oldSelectedIndex = 0;
            itemsInComboBox.Clear();
            // Aantal claimbare slots is het aantal slots tot het volgende onclaimbare slot met een maximum van 8
            var amountOfDurationTimeStrings =
                new[] { amountOfSlotsToNextUnclaimableSlot, AmountOfAvailableQuarters }.Min();
            // Vul de combobox met afschrijvingsduur-strings
            GenerateClaimableDurationStrings(amountOfDurationTimeStrings).ForEach(claimableDurationString =>
                itemsInComboBox.Add(claimableDurationString));
            var amountOfItems = itemsInComboBox.Count;
            // Als de dag helemaal vol zit, dan is het aantal duurstrings 0
            if (amountOfItems == 0)
            {
                ReservationDurationComboBox.SelectedValue = "(geen slots meer reserveerbaar vandaag)";
            }
            else
            {
                // Zo nee, stel dan de geselecteerde index opnieuw in.
                // Het wordt de vorige geselecteerde index,
                // tenzij de vorige geselecteerde index te hoog is
                // omdat de bijbehorende string niet meer in de combobox zit
                var highestSelectableIndex = amountOfItems - 1;
                ReservationDurationComboBox.SelectedIndex = new[] { oldSelectedIndex, highestSelectableIndex }.Min();
            }
        }
 
        // Maak de strings voor de afschrijvings-tijdsduur
        private List<string> GenerateClaimableDurationStrings(int amountOfSlotsToBeClaimed)
        {
            var claimableDurationStrings = new List<string>();
            for (var i = 1; i <= amountOfSlotsToBeClaimed; i++)
            {
                // Bij bijvoorbeeld amountOfSlotsToBeClaimed = 2 maak je 00:15 en 00:30
                var slotString = $"0{i / 4}:{i % 4 * 15}";
                // Bij 01:0 en 02:0 moet je nog een 0 toevoegen zodat je 01:00 en 02:00 krijgt
                if (i % 4 == 0) slotString += "0";
                claimableDurationStrings.Add(slotString);
            }
            return claimableDurationStrings;
        }
 
        // Maak de groene slots op basis van het aantal slots dat je wil claimen
        private List<DateTime> GetAboutToBeClaimedSlots(DateTime displayedDate, int amountOfSlotsToBeClaimed)
        {
            DateTime startSlot;
            try
            {
                // Als de starttijd niet kan worden gelezen...
                startSlot = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
            }
            catch (FormatException)
            {
                // dan betekent dat dat de dag geen vrije slots meer bevat.
                // Het aantal slots dat geclaimd moet worden is dus 0.
                // De lijst met slots is dus leeg.
                return new List<DateTime>();
            }
            var aboutToBeClaimedSlots = new List<DateTime>();
            var startSlotDayQuarter = DateTimeToDayQuarter(startSlot);
            var endSlotDayQuarter = startSlotDayQuarter + amountOfSlotsToBeClaimed;
 
            // Maak slots vanaf de startslot tot de eindslot
            for (var i = startSlotDayQuarter; i < endSlotDayQuarter; i++)
                aboutToBeClaimedSlots.Add(DayQuarterToDateTime(displayedDate, i));
 
            // en retourneer ze
            return aboutToBeClaimedSlots;
        }
 
        // Vul de afschrijvingsstartcombobox
        private void PopulateStartTimeComboBox(DateTime selectedDate, DateTime earliestSlot, DateTime firstDarknessSlot,
            List<DateTime> claimedSlotsForThisDay)
        {
            _reservationStartComboBox.Items.Clear();
            // Maak claimbare startslots op basis van de geselecteerde datum.
            // Zorg er daarbij voor dat alles vóór en na het donker en de geclaimde slots niet geselecteerd kunnen worden:
            // deze startslots worden ook niet gemaakt
            GenerateClaimableStartSlots(selectedDate, earliestSlot, firstDarknessSlot, claimedSlotsForThisDay)
                .ForEach(claimableStartSlot => _reservationStartComboBox.Items.Add(claimableStartSlot.ToString("t")));
            // Als er niets in zit,
            if (_reservationStartComboBox.Items.Count == 0)
            {
                // dan is de hele dag dus al gevuld.
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
 
        // Maak de startslots
        // Zorg er daarbij voor dat alles vóór en na het donker en de geclaimde slots niet geselecteerd kunnen worden:
        private List<DateTime> GenerateClaimableStartSlots(DateTime selectedDate,
            DateTime earliestSlot,
            DateTime firstDarknessSlot,
            List<DateTime> unavailableSlots)
        {
            var claimableSlots = new List<DateTime>();
            var earliestSlotDayQuarter = DateTimeToDayQuarter(earliestSlot);
            var firstDarknessSlotDayQuarter = DateTimeToDayQuarter(firstDarknessSlot);
            // Maak een lijst startslots
            // die begint bij zonsopgang,
            // eindigt bij zonsondergang en
            // geen slots bevat die al gereserveerd zijn
            for (var i = earliestSlotDayQuarter; i < firstDarknessSlotDayQuarter; i++)
                if (!unavailableSlots.Exists(unavailableSlot => DateTimeToDayQuarter(unavailableSlot) == i))
                    claimableSlots.Add(DayQuarterToDateTime(selectedDate, i));
            return claimableSlots;
        }
 
        // Verkrijg de eerste slot waar het gedurende de hele slot licht is
        public DateTime GetFirstLightSlot(DateTime sunrise) => TopRoundTimeToSlot(sunrise);
 
        // Rond een tijdstip naar boven af op kwartieren
        private DateTime TopRoundTimeToSlot(DateTime time) => time.AddMinutes(15 - time.Minute % 15);
 
        // Verkrijg de laatste slot waar het gedurende de hele slot licht is
        public DateTime GetFirstDarknessSlot(DateTime sunset) => BottomRoundTimeToSlot(sunset);
 
        // Rond een tijdstip naar beneden af op kwartieren
        private DateTime BottomRoundTimeToSlot(DateTime time) => time.AddMinutes(-(time.Minute % 15));
 
        // Maak de registreerknop. Standaard staat ie op onklikbaar
        public void MakeRegisterBtn()
        {
            _okButton = new Button { Name = "okBtn", Content = "Afschrijven", Width = 120, Height = 25, IsEnabled = false };
            _okButton.Click += OkBtn_Click;
            _okButton.HorizontalAlignment = HorizontalAlignment.Left;
            _okButton.Margin = new Thickness(150, 120, 0, 0);
            Grid.Children.Add(_okButton);
 
        }
 
        // Tel de tijdsduur bij de starttijd op en genereer zo de eindtijd
        public DateTime GenerateEndTime(DateTime startTime) => startTime
            .AddHours(int.Parse(ReservationDurationComboBox.SelectedValue.ToString()[1].ToString()))
            .AddMinutes(int.Parse(ReservationDurationComboBox.SelectedValue.ToString().Substring(3)));
 
        // Zodra de OkBtn is aangeklikt zal de boot worden afgeschreven na messagebox dialoog bevestiging
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            // Toon een popup.
            // Als de gebruiker de afschrijving toch niet definitief wil maken, dan gaan we weer naar het afschrijvenscherm
            if (MessageBox.Show("Wilt u uw afschrijving definitief maken?",
                    "Afschrijving bevestigen",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) !=
                MessageBoxResult.Yes)
                return;
 
            // Zo nee, dan gaan we de afschrijving in de database zetten
            using (var context = new DataBase())
            {
                // Pak de boot die je wil afschrijven
                var boat = (from db in context.Boats
                            where db.Name.Equals((string)BoatNamesComboBox.SelectedValue)
                            select db).First();
                // Pak de start- en eindtijd
                var selectedDate = BoatTypeTabItemCalendar.SelectedDate.Value.Date;
                var selectedTime = DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString());
                var startTime = selectedDate.AddHours(selectedTime.Hour).AddMinutes(selectedTime.Minute);
                var endTime = GenerateEndTime(startTime);
 
                // Maak een reservering met de geselecteerde boot, de ingelogde gebruiker, de start- en de eindtijd
                context.Reservations.Add(new Reservation(boat, startTime, endTime));
                context.SaveChanges();
            }
            MessageBox.Show("De boot is succesvol afgeschreven",
                "Melding",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            Switcher.Switch(new Dashboard());
        }
 
        // Als er op de combobox voor de bootnamen wordt geklikt...
        private void OnBoatNamesComboBoxClicked(object sender, EventArgs e)
        {
            // Ververs de plannergrid, de afschrijvingsstart-combobox en de -duurcombobox
            FullRefresh();
 
            // Werk de boatview bij met data uit deze boot
            var selectedBoatString = (string)BoatNamesComboBox.SelectedValue;
            var selectedBoat = new BoatController().GetBoatWithName(selectedBoatString);
            BoatView.UpdateView(selectedBoat);
        }
 
        // Verkrijg de zonsopgangs- en zonsondergangstijden voor een geselecteerde datum
        // De applicatie gaat er vanuit dat de roeivereniging zich in hartje Zwolle bevindt
        // Retourneert een array met twee elementen: de datum voor zonsopgang en zonsondergang
        public DateTime[] GetSunriseAndSunsetTimes(DateTime selectedDate)
        {
            // Maak twee booleans en twee DateTime-objecten aan. De inhoud van deze objecten maakt nog niet uit.
            var isSunrise = false;
            var isSunset = false;
            var sunrise = DateTime.Now;
            var sunset = DateTime.Now;
 
            // De methode SunTimes.Instance.CalculateSunRiseSetTimes vormt deze objecten nu om.
            // Met de booleans doen we niets, ze worden alleen gebruikt omdat dat nodig is voor de methode
            SunTimes.Instance.CalculateSunRiseSetTimes(
                // Breedtegraad voor Zwolle
                new SunTimes.LatitudeCoords(
                    52,
                    31,
                    0,
                    SunTimes.LatitudeCoords.Direction.North),
                // Lengtegraad voor Zwolle
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
 
            // sunrise bevat nu de tijd van zonsopgang.
            // sunset bevat nu de tijd van zonsondergang.
 
            // Retourneer een array met sunrise en sunset
            return new[] { sunrise, sunset };
        }
 
        // Als er op de kalender wordt geklikt, verversen we de afschrijvingsstart-combobox, de -duurcombobox en de plannergrid
        private void OnCalendarClicked(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => FullRefresh();
 
        // Ververs de afschrijvingsstart-combobox, de -duurcombobox en de plannergrid
        private void FullRefresh()
        {
            // Eerst ververs je de starttijd.
            // Als je van datum verandert, kan de starttijd namelijk veranderen.
            // De duurcombobox moet weten of de startcombobox veranderd is, maar andersom maakt niet uit.
            // Dan wordt de duurcombobox aangepast.
            // Als de duur wordt veranderd, dan moet de plannergrid dat weten.
            // Daarom wordt dán pas de plannergrid bijgewerkt
            var selectedDate = BoatTypeTabItemCalendar.SelectedDate;
            if (!selectedDate.HasValue) return;
 
            // Pak de DateTime uit de DateTime? van de kalender
            var selectedDateValue = selectedDate.Value;
 
            // Ververs de afschrijvingsstart-combobox.
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(selectedDateValue);
            var earliestSlot = GetFirstLightSlot(sunriseAndSunsetTimes[0]);
 
            var firstDarknessSlot = GetFirstDarknessSlot(sunriseAndSunsetTimes[1]);
 
            var selectedBoat = (string)BoatNamesComboBox.SelectedValue;
            var claimedSlotsForThisDayAndBoat =
                GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime.Now, selectedDateValue, selectedBoat,
                    earliestSlot, firstDarknessSlot);
 
            PopulateStartTimeComboBox(selectedDateValue, earliestSlot, firstDarknessSlot, claimedSlotsForThisDayAndBoat);
 
            // Ververs de afschrijvingsduur-combobox
            var amountOfClaimableSlots = GetAmountOfClaimableSlots(claimedSlotsForThisDayAndBoat, firstDarknessSlot);
 
            PopulateDurationTimeComboBox(amountOfClaimableSlots);
 
            // Ververs de plannergrid
            var amountOfSlotsWantingToBeClaimed = GetAmountOfSlotsWantingToBeClaimed();
            var amountOfSlotsToBeClaimed =
                GetAmountOfSlotsToBeClaimed(amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed);
            var aboutToBeClaimedSlots = GetAboutToBeClaimedSlots(selectedDateValue, amountOfSlotsToBeClaimed);
 
            PlannerGrid.Populate(earliestSlot, firstDarknessSlot, claimedSlotsForThisDayAndBoat, aboutToBeClaimedSlots);
        }
 
        // Pak het aantal plannerslots dat geclaimd moet worden.
        // Dit is het aantal slots dat geclaimd wil worden volgens de afschrijvingsduur-combobox,
        // maar het mag niet meer zijn dan het aantal slots dat geclaimd mag worden
        private int GetAmountOfSlotsToBeClaimed(int amountOfClaimableSlots, int amountOfSlotsWantingToBeClaimed) =>
            new[] { amountOfClaimableSlots, amountOfSlotsWantingToBeClaimed }.Min();
 
        // Verkrijg de slots die
        // 1) waarbij de geselecteerde boot (we openen het reserveringsscherm net, dus de eerste boot in de botencombobox is de geselecteerde boot) al afgeschreven is
        // 2) al voorbij zijn (maar waar de zon al wel op was)
        // 3) te ver in de toekomst zijn om geclaimd te kunnen worden (maar waar de zon nog niet onder is)
        public List<DateTime> GetClaimedPastAndTooDistantSlotsForThisDayAndBoat(DateTime now, DateTime selectedDate, string selectedBoatString,
            DateTime earliestSlot, DateTime firstDarknessSlot)
        {
            var claimedSlots = new List<DateTime>();
 
            // Verkrijg de reserveringen voor de geselecteerde boot.
            new ReservationController()
                .GetReservationsForDayAndBoatThatAreNotDeletedOrBroken(selectedDate,
                    new BoatController().GetBoatWithName(selectedBoatString)).ForEach(reservation =>
 
                    // Zet de reserveringen in de database om in slots
                    {
                        for (var i = DateTimeToDayQuarter(reservation.Start);
                        i < DateTimeToDayQuarter(reservation.End);
                        i++)
                            claimedSlots.Add(DayQuarterToDateTime(selectedDate, i));
                    });
            var datePartOfDateTimeNow = now.Date;
 
            // Als de geselecteerde dag vandaag is, dan moeten er ook nog slots die al voorbij zijn worden grijs gemaakt
            // Voeg slots die voorbij zijn toe aan de geclaimde slots
            // Als de geselecteerde dag overmorgen is
            // (of het ingelogde lid is wedstrijdcommissaris en de geselecteerde dag is over een jaar),
            // dan moeten er ook nog slots die te ver weg zijn worden grijs gemaakt
            // Voeg slots die te ver weg zijn aan de geclaimde slots
            // In de andere gevallen, retourneer alleen de geclaimde slots
 
 
            if (selectedDate.Date.Equals(datePartOfDateTimeNow))
                return GetClaimedAndPastSlots(claimedSlots, now, earliestSlot, firstDarknessSlot);
            else if (DateIsLastReservableDate(new UserController().LoggedInUserIsRaceCommissioner(),
                datePartOfDateTimeNow,
                selectedDate))
                return GetClaimedAndTooDistantSlots(claimedSlots, selectedDate, now);
            else return claimedSlots;
        }
 
        // Controleer of de datum die nu geselecteerd is, de laatst mogelijke datum is
        private bool DateIsLastReservableDate(bool loggedInUserIsRaceCommissioner, DateTime now, DateTime selectedDate)
        {
            var latestDateThatThisUserMayReserve = loggedInUserIsRaceCommissioner ? now.AddYears(1) : now.AddDays(2);
            return selectedDate.Date.Equals(latestDateThatThisUserMayReserve);
        }
 
        // Voeg slots die voorbij zijn toe aan de geclaimde slots
        private List<DateTime> GetClaimedAndPastSlots(List<DateTime> claimedSlots, DateTime now, DateTime earliestSlot,
            DateTime firstDarknessSlot)
        {
            var claimedAndPastSlots = new List<DateTime>(claimedSlots);
            var earliestSlotDayQuarter = DateTimeToDayQuarter(earliestSlot);
            var slotThatIsNow = DateTimeToDayQuarter(TopRoundTimeToSlot(now));
            var firstDarknessSlotDayQuarter = DateTimeToDayQuarter(firstDarknessSlot);
 
            // De laatste slot die als "voorbij" moet worden gemarkeerd is de slot van dit moment,
            // maar de laatste grijze slot mag niet voorbij de laatste slot waar het nog licht is komen.
            var latestSlotThatShouldBeMarkedAsDue = new[] { slotThatIsNow, firstDarknessSlotDayQuarter }.Min();
 
            // Meng de slots in het verleden samen met de slots die al geclaimd zijn.
            // Als er al een slot in het verleden geclaimd is,
            // dan wordt die niet dubbel grijs gemaakt
            for (var i = earliestSlotDayQuarter; i < latestSlotThatShouldBeMarkedAsDue; i++)
            {
                var passedSlot = DayQuarterToDateTime(now, i);
                if (!claimedSlots.Contains(passedSlot)) claimedAndPastSlots.Add(passedSlot);
            }
 
            return claimedAndPastSlots;
        }
 
        // Voeg slots die te ver weg zijn aan de geclaimde slots
        public List<DateTime> GetClaimedAndTooDistantSlots(List<DateTime> claimedSlots, DateTime lastReservableDay, DateTime now)
        {
            var claimedAndTooDistantSlots = new List<DateTime>(claimedSlots);
            var sunriseAndSunsetTimesForTwoDaysFromNow = GetSunriseAndSunsetTimes(lastReservableDay);
            var earliestSlotTwoDaysFromNow = GetFirstLightSlot(sunriseAndSunsetTimesForTwoDaysFromNow[0]);
            var firstDarknessSlotTwoDaysFromNow = GetFirstDarknessSlot(sunriseAndSunsetTimesForTwoDaysFromNow[1]);
            var earliestSlotDayQuarter = DateTimeToDayQuarter(earliestSlotTwoDaysFromNow);
            var lastReservableDaySameTimeAsNow = lastReservableDay.AddHours(now.Hour).AddMinutes(now.Minute);
            var slotThatIsTwoDaysFromNow = DateTimeToDayQuarter(BottomRoundTimeToSlot(lastReservableDaySameTimeAsNow));
 
            // De vroegste slot die als "te ver in de toekomst" moet worden gemarkeerd is de slot van dit tijdstip over twee dagen,
            // maar de eerste grijze slot mag niet voor de eerste slot waar het al licht is komen.
            var earliestSlotThatShouldBeMarkedAsTooDistant =
                new[] { earliestSlotDayQuarter, slotThatIsTwoDaysFromNow }.Max();
            var firstDarknessSlotDayQuarter = DateTimeToDayQuarter(firstDarknessSlotTwoDaysFromNow);
 
            // Meng de slots te ver in de toekomst samen met de slots die al geclaimd zijn.
            // Als er al een slot te ver in de toekomst geclaimd is,
            // dan wordt die niet dubbel grijs gemaakt
            for (var i = earliestSlotThatShouldBeMarkedAsTooDistant; i < firstDarknessSlotDayQuarter; i++)
            {
                var tooDistantSlot = DayQuarterToDateTime(lastReservableDay, i);
                if (!claimedSlots.Contains(tooDistantSlot)) claimedAndTooDistantSlots.Add(tooDistantSlot);
            }
 
            return claimedAndTooDistantSlots;
        }
 
        // Zet een datum om naar een dagkwartier. Het dagkwartier van 8.30 is 34
        // (8 uren * 4 kwartieren per uur = 32 kwartieren,
        // 30 minuten / 15 minuten per kwartieren = 2 kwartieren,
        // bij elkaar opgeteld is 34)
        public int DateTimeToDayQuarter(DateTime time) => time.Hour * 4 + time.Minute / 15;
 
        // Zet een dagkwartier om naar een datumtijd. De tijd van dagkwartier 34 is 8.30
        // (34 kwartieren / 4 kwartieren per uur = 8 uren rest 2,
        // 2 kwartieren * 15 minuten per kwartier = 30 minuten,
        // bij elkaar opgeteld is 8 uur 30 minuten = 8.30)
        public DateTime DayQuarterToDateTime(DateTime selectedDate,
            int dayQuarter) => new DateTime(selectedDate.Year,
            selectedDate.Month,
            selectedDate.Day,
            dayQuarter / 4,
            dayQuarter % 4 * 15,
            00);
 
        private void OnPlannerGridClick(object sender, MouseButtonEventArgs e)
        {
//            MessageBox.Show("Hoi");
            var position = e.GetPosition(PlannerGrid);
            var x = position.X;
            var y = position.Y;
            if (x >= 0 && x < 200 && y >= 0 && _reservationStartComboBox.SelectedIndex >= 0 && ReservationDurationComboBox.SelectedIndex >= 0)
            {
                var minutes = PlannerGrid.GetMinutesFromX(x);
                var hour = PlannerGrid.GetHourFromY(y);
                _selectsDuration = !_selectsDuration;
                if (_selectsDuration)
                {
//                    MessageBox.Show($"{DateTime.Today.AddHours(hour).AddMinutes(minutes).ToLongTimeString()}, selecteert duur");
                    var selectedIndex = SelectStart(hour, minutes);
                    if (selectedIndex.HasValue)
                    {
                        ReservationDurationComboBox.SelectedIndex = 0;
                        _reservationStartComboBox.SelectedIndex = selectedIndex.Value;
                        OnStartComboBoxClick(null, null);
                    }
                }
                else
                {
//                    MessageBox.Show($"{DateTime.Today.AddHours(hour).AddMinutes(minutes).ToLongTimeString()}, selecteert begin");
                    var selectedIndex = SelectDuration(hour, minutes, DateTime.Parse(_reservationStartComboBox.SelectedValue.ToString()));
                    if (selectedIndex.HasValue)
                    {
                        ReservationDurationComboBox.SelectedIndex = selectedIndex.Value;
                        OnDurationComboBoxClick(null, null);
                    }
                }
 
            }
            else
            {
                _selectsDuration = false;
            }
        }
 
        private int? SelectDuration(int hour, int minutes, DateTime selectedStartSlot)
        {
            var items = ReservationDurationComboBox.Items;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var extraHours = int.Parse(item.ToString().Substring(0, 2));
                var extraMinutes = int.Parse(item.ToString().Substring(3, 2));
                var endSlot = selectedStartSlot.AddHours(extraHours).AddMinutes(extraMinutes).AddMinutes(-15);
                if (endSlot.Hour == hour && endSlot.Minute == minutes)
                {
                    return i;
                }
            }
 
            return null;
        }
 
        private int? SelectStart(int hour, int minutes)
        {
            var items = _reservationStartComboBox.Items;
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var startSlot = DateTime.Parse(item.ToString());
                if (startSlot.Hour == hour && startSlot.Minute == minutes)
                {
                    return i;
                }
            }
 
            return null;
        }
    }
}