using Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Reserve
{
    public class PlannerGrid : Canvas
    {
        private const int PlannerGridColumnWidth = 50;
        private const int PlannerGridRowHeight = 20;
        private const int PlannerGridWidth = 200;
        private readonly Brush _slotDisabledDueToDarknessColor = new SolidColorBrush(Colors.DarkRed);
        private readonly Brush _slotClaimedColor = new SolidColorBrush(Colors.Gray);
        private readonly Brush _slotAboutToBeClaimedColor = new SolidColorBrush(Colors.Green);
        private int _earliestHourOnPlanningGrid;

        public PlannerGrid()
        {
            HorizontalAlignment = HorizontalAlignment.Right;
            VerticalAlignment = VerticalAlignment.Top;
            Width = PlannerGridWidth;
            var l = new Label { Content = "X" };
            l.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var margin = l.Margin;
            margin.Top = l.DesiredSize.Height;
            Margin = margin;
        }

        private List<Line> MakePlannerGridColumnDividers(int plannerGridHeight)
        {
            var plannerGridColumnDividers = new List<Line>();
            for (var i = 0;
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
        public void Populate(DateTime earliestSlot, DateTime latestSlot, List<DateTime> claimedSlots)
        {
            _earliestHourOnPlanningGrid = GetEarliestHourOnPlanningGrid(earliestSlot);
            var latestHourOnPlanningGrid = GetLatestHourOnPlanningGrid(latestSlot);
            var amountOfHoursOnDisplay = latestHourOnPlanningGrid - _earliestHourOnPlanningGrid;
            Height = GeneratePlannerGridHeight(amountOfHoursOnDisplay);
            Children.Clear();
            GeneratePlannerSideLabels(_earliestHourOnPlanningGrid, latestHourOnPlanningGrid)
                .ForEach(plannerSideLabel => Children.Add(plannerSideLabel));
            var topLabels = GeneratePlannerTopLabels();
            topLabels.ForEach(plannerTopLabel => Children.Add(plannerTopLabel));
            Margin = new Thickness(Margin.Left, Margin.Top, topLabels[topLabels.Count - 1].DesiredSize.Width / 2,
                Margin.Bottom);
            GeneratePlannerHorizontalLines(_earliestHourOnPlanningGrid, latestHourOnPlanningGrid)
                .ForEach(plannerHorizontalLine => Children.Add(plannerHorizontalLine));
            MakePlannerGridColumnDividers(
                amountOfHoursOnDisplay * PlannerGridRowHeight)
                .ForEach(plannerGridColumnDivider => Children.Add(plannerGridColumnDivider));
            GetFirstUnavailableSlotTiles(earliestSlot, _earliestHourOnPlanningGrid)
                .ForEach(firstUnavailableSlotTile => Children.Add(firstUnavailableSlotTile));
            GetLastUnavailableSlotTiles(latestSlot, _earliestHourOnPlanningGrid)
                .ForEach(lastUnavailableSlotTile => Children.Add(lastUnavailableSlotTile));
            GetClaimedSlotTiles(claimedSlots, _earliestHourOnPlanningGrid)
                .ForEach(claimedSlotTile => Children.Add(claimedSlotTile));
        }

        public void Populate(DateTime earliestSlot, DateTime latestSlot, List<DateTime> claimedSlots,
            List<DateTime> slotsAboutToBeClaimed)
        {
            Populate(earliestSlot, latestSlot, claimedSlots);
            GetAboutToBeClaimedSlotTiles(slotsAboutToBeClaimed, _earliestHourOnPlanningGrid)
                .ForEach(slotAboutToBeClaimedTile => Children.Add(slotAboutToBeClaimedTile));
        }

        private List<Rectangle> GetAboutToBeClaimedSlotTiles(List<DateTime> slotsAboutToBeClaimed, int firstHour) =>
            slotsAboutToBeClaimed.ConvertAll(slotAboutToBeClaimed =>
                SlotAboutToBeClaimedTile(firstHour, slotAboutToBeClaimed.Hour, slotAboutToBeClaimed.Minute / 15));

        private Rectangle SlotAboutToBeClaimedTile(int firstHour, int slotHour, int slotQuarter) =>
            OccupiedSlotTile(firstHour, slotHour, slotQuarter, _slotAboutToBeClaimedColor);

        private List<Rectangle> GetClaimedSlotTiles(List<DateTime> claimedSlots, int firstHour) =>
            claimedSlots.ConvertAll(
                claimedSlot => ClaimedSlotTile(firstHour, claimedSlot.Hour, claimedSlot.Minute / 15));

        private int GeneratePlannerGridHeight(int amountOfHoursOnDisplay) =>
            amountOfHoursOnDisplay * PlannerGridRowHeight;

        private List<Label> GeneratePlannerSideLabels(int earliestHourOnPlanningGrid, int latestHourOnPlanningGrid)
        {
            var plannerSideLabels = new List<Label>();
            for (var i = earliestHourOnPlanningGrid; i <= latestHourOnPlanningGrid; i++)
            {
                var hourLabel = new Label { Content = $"{i}.00" };
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
            var plannerTopLabels = new List<Label>();
            var minutes = 0;
            for (var i = 0; i <= PlannerGridWidth; i += PlannerGridColumnWidth)
            {
                var quarterHourLabel = new Label { Content = $"xx:{minutes}" };
                minutes += 15;
                quarterHourLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var margin = quarterHourLabel.Margin;
                margin.Left = i - quarterHourLabel.DesiredSize.Width / 2;
                margin.Top = -quarterHourLabel.DesiredSize.Height;
                quarterHourLabel.Margin = margin;
                plannerTopLabels.Add(quarterHourLabel);
            }
            return plannerTopLabels;
        }

        private List<Line> GeneratePlannerHorizontalLines(int earliestHourOnPlanningGrid, int latestHourOnPlanningGrid)
        {
            var plannerHorizontalLines = new List<Line>();
            for (var i = earliestHourOnPlanningGrid; i <= latestHourOnPlanningGrid; i++)
            {
                var hourLine = new Line
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

        private int GetEarliestHourOnPlanningGrid(DateTime earliestSlot) => earliestSlot.Hour;

        private List<Rectangle> GetLastUnavailableSlotTiles(DateTime latestSlot, int firstHour)
        {
            var lastUnavailableSlotTiles = new List<Rectangle>();
            var amountOfLastUnavailableSlots = 4 - latestSlot.Minute / 15;
            var latestHour = latestSlot.Hour;
            for (var i = 3; i > 3 - amountOfLastUnavailableSlots; i--)
                lastUnavailableSlotTiles.Add(SlotInDarknessTile(firstHour, latestHour, i));
            return lastUnavailableSlotTiles;
        }
        private List<Rectangle> GetFirstUnavailableSlotTiles(DateTime earliestSlot, int firstHour)
        {
            var firstUnavailableSlotTiles = new List<Rectangle>();
            var amountOfFirstUnavailableSlots = earliestSlot.Minute / 15;
            for (var i = 0; i < amountOfFirstUnavailableSlots; i++)
                firstUnavailableSlotTiles.Add(SlotInDarknessTile(firstHour, firstHour, i));
            return firstUnavailableSlotTiles;
        }

        private Rectangle SlotInDarknessTile(int firstHour, int disabledSlotHour, int quarter) =>
            OccupiedSlotTile(firstHour, disabledSlotHour, quarter, _slotDisabledDueToDarknessColor);

        private Rectangle ClaimedSlotTile(int firstHour, int claimedSlotHour, int quarter) =>
            OccupiedSlotTile(firstHour, claimedSlotHour, quarter, _slotClaimedColor);

        private Rectangle OccupiedSlotTile(int firstHour, int occupiedSlotHour, int quarter, Brush color) => new Rectangle
        {
            Height = PlannerGridRowHeight,
            Width = PlannerGridColumnWidth,
            Fill = color,
            Margin = new Thickness(
                quarter * PlannerGridColumnWidth,
                (occupiedSlotHour - firstHour) * PlannerGridRowHeight,
                0,
                0)
        };
    }
}