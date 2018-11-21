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
        public List<Boat> Boats { get; set; }
        public Boat.BoatType BoatType { get; set; }
        public ComboBox cbTimes = new ComboBox();
        public ComboBox cbNames = new ComboBox();

        public BoatTypeTabItem(/*List<Boat> boatsFromThisType, */Boat.BoatType type, List<Reservation> reservations)
        {
            BoatType = type;
            Header = type.ToString();
            Reservations = reservations;
            Grid = new Grid();
            Calendar = MakeCalendar();
            Grid.Children.Add(Calendar);
            var sunriseAndSunsetTimes = GetSunriseAndSunsetTimes(DateTime.Now);
            PlannerGrid = new PlannerGrid();
            PlannerGrid.Populate(sunriseAndSunsetTimes);
            Grid.Children.Add(PlannerGrid);
            Content = Grid;
            FillComboNames();
            FillComboTimes();
            MakeRegisterBtnVisibleAfterChoice();
        }

        public void FillComboTimes()
        {
            cbTimes.Name = "ComboTimes";
            cbTimes.Width = 120;
            cbTimes.Height = 25;
            cbTimes.HorizontalAlignment = HorizontalAlignment.Left;
            cbTimes.Margin = new Thickness((90 + cbTimes.Width), 123, 0, 0);
            cbTimes.SelectedIndex = 0;
            cbTimes.Items.Add("00:15");
            cbTimes.Items.Add("00:30");
            cbTimes.Items.Add("00:45");
            cbTimes.Items.Add("01:00");
            cbTimes.Items.Add("01:15");
            cbTimes.Items.Add("01:30");
            cbTimes.Items.Add("01:45");
            cbTimes.Items.Add("02:00");
            Grid.Children.Add(cbTimes);

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
            okBtn.Margin = new Thickness((225 + okBtn.Width), 123, 0, 0);
            Grid.Children.Add(okBtn);
        }

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
                timeEnd = int.Parse(newTime) + 60;

            }

            // if hours is >= 2

            else timeEnd = 120;

            return timeEnd;
        }

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

                    //Boat b1 = new Boat();
                    using (Database context = new Database())
                    {
                        var boat = (from db in context.Boats
                                      where db.Name.Equals((string) cbNames.SelectedValue)
                                      select db).First();
                        //var x = BoatTypeTabControl.SelectedItem;
                        //BoatTypeTabItem y = (BoatTypeTabItem)x;
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

        public void FillComboNames()
        {
            cbNames.Name = "ComboBoatName";
            cbNames.Tag = 
            cbNames.Width = 120;
            cbNames.Height = 25;
            cbNames.HorizontalAlignment = HorizontalAlignment.Left;
            cbNames.Margin = new Thickness(45, 123, 0, 0);
            cbNames.SelectedIndex = 0;
            Grid.Children.Add(cbNames);

            using (Database context = new Database())
                foreach (var item in from db in context.Boats where db.Type == BoatType select db.Name) cbNames.Items.Add(item);
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