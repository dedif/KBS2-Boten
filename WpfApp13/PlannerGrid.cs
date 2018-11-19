using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ConsoleApp1;

namespace WpfApp6
{
    public class PlannerGrid : Canvas
    {
        private const int PlannerGridColumnWidth = 50;
        private const int PlannerGridRowHeight = 20;
        private const int PlannerGridWidth = 200;
        private readonly Brush _slotDisabledDueToDarknessColor = new SolidColorBrush(Colors.DarkRed);
        public PlannerGrid()
        {
            HorizontalAlignment = HorizontalAlignment.Right;
            VerticalAlignment = VerticalAlignment.Top;
            Width = PlannerGridWidth;
            Label l = new Label { Content = "X" };
            l.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var margin = l.Margin;
            margin.Top = l.DesiredSize.Height;
            Margin = margin;
        }

        private List<Line> MakePlannerGridColumnDividers(int plannerGridHeight)
        {
            List<Line> plannerGridColumnDividers = new List<Line>();
            for (int i = 0;
                i <= PlannerGridWidth;
                i += PlannerGridColumnWidth)
                plannerGridColumnDividers.Add(new Line
                {
                    X1 = i,
                    Y1 = 0,
                    X2 = i,
                    Y2 = plannerGridHeight,
                    Stroke = new SolidColorBrush(Colors.Gray)
                });
            return plannerGridColumnDividers;
        }
        public void Populate(DateTime[] sunriseAndSunsetTimes)
        {
            var earliestSlot = GetEarliestSlot(sunriseAndSunsetTimes[0]);
            var latestSlot = GetLatestSlot(sunriseAndSunsetTimes[1]);
            var earliestHourOnPlanningGrid = GetEarliestHourOnPlanningGrid(earliestSlot);
            var latestHourOnPlanningGrid = GetLatestHourOnPlanningGrid(latestSlot);
            var amountOfHoursOnDisplay = latestHourOnPlanningGrid - earliestHourOnPlanningGrid;
            Height = GeneratePlannerGridHeight(amountOfHoursOnDisplay);
            Children.Clear();
            foreach (var plannerSideLabel in GeneratePlannerSideLabels(earliestHourOnPlanningGrid,
                latestHourOnPlanningGrid))
                Children.Add(plannerSideLabel);
            foreach (var plannerTopLabel in GeneratePlannerTopLabels()) Children.Add(plannerTopLabel);
            foreach (var plannerHorizontalLine in GeneratePlannerHorizontalLines(earliestHourOnPlanningGrid,
                latestHourOnPlanningGrid))
                Children.Add(plannerHorizontalLine);
            foreach (var plannerGridColumnDivider in MakePlannerGridColumnDividers(
                amountOfHoursOnDisplay * PlannerGridRowHeight))
                Children.Add(plannerGridColumnDivider);
			foreach (var firstUnavailableSlotTile in GetFirstUnavailableSlotTiles(earliestSlot,
                earliestHourOnPlanningGrid))
                Children.Add(firstUnavailableSlotTile);
            foreach (var lastUnavailableSlotTile in GetLastUnavailableSlotTiles(latestSlot,
                earliestHourOnPlanningGrid))
                Children.Add(lastUnavailableSlotTile);
        }
		
		public void Populate(List<Boat> boats)
        {
            foreach (var boat in boats)
            {
                var earliestSlot = GetEarliestSlotForBoat(boat);
            }
        }
		
		private DateTime GetEarliestSlotForBoat(Boat boat)
		{
		    return DateTime.Now;
		}

        private int GeneratePlannerGridHeight(int amountOfHoursOnDisplay) =>
            amountOfHoursOnDisplay * PlannerGridRowHeight;

        private List<Label> GeneratePlannerSideLabels(int earliestHourOnPlanningGrid, int latestHourOnPlanningGrid)
        {
            List<Label> plannerSideLabels = new List<Label>();
            for (int i = earliestHourOnPlanningGrid; i <= latestHourOnPlanningGrid; i++)
            {
                Label hourLabel = new Label { Content = $"{i}.00" };
                hourLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var margin = hourLabel.Margin;
                margin.Top = (i - earliestHourOnPlanningGrid) * PlannerGridRowHeight -
                             hourLabel.DesiredSize.Height / 2;
                margin.Left = -hourLabel.DesiredSize.Width;
                hourLabel.Margin = margin;
                plannerSideLabels.Add(hourLabel);
            }

            return plannerSideLabels;
        }
        private List<Label> GeneratePlannerTopLabels()
        {
            List<Label> plannerTopLabels = new List<Label>();
            int minutes = 0;
            for (int i = 0; i < PlannerGridWidth; i += PlannerGridColumnWidth)
            {
                minutes += 15;
                Label quarterHourLabel = new Label { Content = $"xx:{minutes}" };
                quarterHourLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var margin = quarterHourLabel.Margin;
                margin.Left = i;
                margin.Top = -quarterHourLabel.DesiredSize.Height;
                quarterHourLabel.Margin = margin;
                plannerTopLabels.Add(quarterHourLabel);
            }
            return plannerTopLabels;
        }

        private List<Line> GeneratePlannerHorizontalLines(int earliestHourOnPlanningGrid, int latestHourOnPlanningGrid)
        {
            List<Line> plannerHorizontalLines = new List<Line>();
            for (int i = earliestHourOnPlanningGrid; i <= latestHourOnPlanningGrid; i++)
            {
                Line hourLine = new Line
                {
                    X1 = 0,
                    X2 = PlannerGridWidth,
                    Y1 = (i - earliestHourOnPlanningGrid) * PlannerGridRowHeight,
                    Y2 = (i - earliestHourOnPlanningGrid) * PlannerGridRowHeight,
                    Stroke = new SolidColorBrush(Colors.Gray)
                };
                plannerHorizontalLines.Add(hourLine);
            }
            return plannerHorizontalLines;
        }

        private int GetLatestHourOnPlanningGrid(DateTime earliestSlot) => earliestSlot.Hour + 1;

        private DateTime GetLatestSlot(DateTime sunset) => sunset.AddMinutes(-(sunset.Minute % 15));

        private int GetEarliestHourOnPlanningGrid(DateTime earliestSlot) => earliestSlot.Hour;

        private DateTime GetEarliestSlot(DateTime sunrise) => sunrise.AddMinutes(15 - sunrise.Minute % 15);
		
		private List<Rectangle> GetLastUnavailableSlotTiles(DateTime latestSlot, int firstHour)
        {
            Console.WriteLine(latestSlot);
            Console.WriteLine(firstHour);
            List<Rectangle> lastUnavailableSlotTiles = new List<Rectangle>();
            int amountOfLastUnavailableSlots = 4 - latestSlot.Minute / 15;
            Console.WriteLine(amountOfLastUnavailableSlots);
            int latestHour = latestSlot.Hour;
            for (int i = 3; i > 3 - amountOfLastUnavailableSlots; i--)
                lastUnavailableSlotTiles.Add(SlotInDarknessTile(firstHour, latestHour, i));
            return lastUnavailableSlotTiles;
        }
        private List<Rectangle> GetFirstUnavailableSlotTiles(DateTime earliestSlot, int firstHour)
        {
            List<Rectangle> firstUnavailableSlotTiles = new List<Rectangle>();
            int amountOfFirstUnavailableSlots = earliestSlot.Minute / 15;
            for (int i = 0; i < amountOfFirstUnavailableSlots; i++)
                firstUnavailableSlotTiles.Add(SlotInDarknessTile(firstHour, firstHour, i));
            return firstUnavailableSlotTiles;
        }

        private Rectangle SlotInDarknessTile(int firstHour, int disabledSlotHour, int quarter)
        {
            Rectangle slotDisabler = new Rectangle
            {
                Height = PlannerGridRowHeight,
                Width = PlannerGridColumnWidth,
                Fill = _slotDisabledDueToDarknessColor
            };
            slotDisabler.Margin =
                new Thickness(
                    quarter * PlannerGridColumnWidth,
                    (disabledSlotHour - firstHour) * PlannerGridRowHeight,
                    0,
                    0);
            return slotDisabler;
        }
    }
}