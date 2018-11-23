using System;
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
        public ComboBox cbTimes = new ComboBox();
        public ComboBox cbNames = new ComboBox();
        public ComboBox cbStartTimes = new ComboBox();
        public ReservationController reservationController = new ReservationController();
        public BoatTypeTabItem(List<Boat> boats, Boat.BoatType type, List<Reservation> reservations)
        {
            BoatType = type;
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
                Margin = new Thickness(5, 200, 0, 0)
            });
            Grid.Children.Add(new Label
            {
                Content = "Start:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 260, 0, 0)
            });
            Grid.Children.Add(new Label
            {
                Content = "Tijdsduur:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 320, 0, 0)
            });
            Grid.Children.Add(new Label
            {
                Content = "Eigenschappen boot:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(150, 180, 0, 0)
            });
            Grid.Children.Add(new BoatView());
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            PlannerGrid.Populate(sunriseAndSunsetTimes);
            Grid.Children.Add(PlannerGrid);
            Content = Grid;
            FillComboNames();
            FillComboTimes();
            GetAvailableStartTimeSlotsFromDatabase();
            MakeRegisterBtnVisibleAfterChoice();
        }


        // this method will fill the comboBox with lengths of 15 mins
        public void FillComboTimes()
        {
            cbTimes.Name = "ComboTimes";
            cbTimes.Width = 120;
            cbTimes.Height = 25;
            cbTimes.HorizontalAlignment = HorizontalAlignment.Left;
            cbTimes.Margin = new Thickness(10, 240, 0, 0);
            cbTimes.SelectedIndex = 0;
            //for (int i = 15; i <= 120; i+= 15) cbTimes.Items.Add($"0{Math.Floor((double)i / 60)}:{Math.Round((double)i % 60, 2)}");
            foreach (var time in new[] {"00:15", "00:30", "00:45", "01:00", "01:15", "01:30", "01:45", "02:00" }) cbTimes.Items.Add(time);
            Grid.Children.Add(cbTimes);
        }



        public void GetAvailableStartTimeSlotsFromDatabase()
        {
            cbStartTimes.Name = "ComboStartTime";
            cbStartTimes.Width = 120;
            cbStartTimes.Height = 25;
            cbStartTimes.HorizontalAlignment = HorizontalAlignment.Left;
            cbStartTimes.Margin = new Thickness(10, 120, 0, 0);
            cbStartTimes.SelectedIndex = 0;
            List<DateTime> lijst = reservationController.GetAvailableSlots(DateTime.Now);
            foreach (var item in lijst)
            {
                cbStartTimes.Items.Add(item);
            }
            
        }

        // This method fills the comboBox with the names of boats that are existing in the DB.
        public void FillComboNames()
        {
            cbNames.Name = "ComboBoatName";
            cbNames.Width = 120;
            cbNames.Height = 25;
            cbNames.HorizontalAlignment = HorizontalAlignment.Left;
            cbNames.Margin = new Thickness(10, 0, 0, 0);
            cbNames.SelectedIndex = 0;
            Grid.Children.Add(cbNames);

            using (Database context = new Database())
                foreach (var item in from db in context.Boats where db.Type == BoatType select db.Name) cbNames.Items.Add(item);
        }

        public void MakeRegisterBtnVisibleAfterChoice()
        {
            Button okBtn = new Button();
            okBtn.Name = "okBtn";
            okBtn.Content = "Afschrijven";
            okBtn.Width = 120;
            okBtn.Height = 25;
            okBtn.Click += OkBtn_Click;
            okBtn.HorizontalAlignment = HorizontalAlignment.Left;
            okBtn.Margin = new Thickness(150, 240, 0, 0);
            Grid.Children.Add(okBtn);
        }

        // This method will take the length of the reserve period from the selected comboboxItem
        public int CalculateQuarterFromComboBox()
        {
            int timeEnd = 0;
            string oldTime = cbTimes.Text;

            // if hours is < 1
            if (oldTime == "00:15" || oldTime == "00:30" || oldTime == "00:45")
            {
                char[] MyChar = { '0', '0', ':' };
                string newTime = oldTime.TrimStart(MyChar);
                timeEnd = int.Parse(newTime);
            }

            // if hours is >= 1 && < 2
            else if (oldTime == "01:00" || oldTime == "01:15" || oldTime == "01:30" || oldTime == "01:45")
            {
                char[] MyChar = { '0', '1', ':' };
                string newTime = oldTime.TrimStart(MyChar);
                timeEnd = int.Parse(newTime + 60);

            }

            // if hours is >= 2
            else timeEnd = 120;

            return timeEnd;
        }


        // When the button is clicked, boats will be reserved after messagebox dialog comfirmation
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Succes = MessageBox.Show(
                            "Wilt u uw afschrijving definitief maken?",
                            "Afschrijving bevestigen",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);
            switch (Succes)
            {
                case MessageBoxResult.Yes:

                    using (Database context = new Database())
                    {
                        var boat = (from db in context.Boats
                                      where db.Name.Equals((string) cbNames.SelectedValue)
                                      select db).First();
                        Reservation rs1 = new Reservation(boat, new Member(), /*y.Calendar.SelectedDate.Value*/DateTime.Now, DateTime.Now.AddMinutes(CalculateQuarterFromComboBox()));
                        context.Reservations.Add(rs1);
                        context.SaveChanges();

                        MessageBoxResult Saved = MessageBox.Show(
                            "De boot is succesvol afgeschreven",
                            "Melding",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    break;

                case MessageBoxResult.No:
                    break;

            }


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