using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp6
{
    public class PlannerGrid : Canvas
    {
        private int _plannerGridColumnWidth = 50;
        private int _plannerGridRowHeight = 20;
        private int _plannerGridWidth = 200;
        public PlannerGrid()
        {
            HorizontalAlignment = HorizontalAlignment.Right;
            VerticalAlignment = VerticalAlignment.Top;
            Width = _plannerGridWidth;
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
                i <= _plannerGridWidth;
                i += _plannerGridColumnWidth)
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
                amountOfHoursOnDisplay * _plannerGridRowHeight))
                Children.Add(plannerGridColumnDivider);
        }

        private int GeneratePlannerGridHeight(int amountOfHoursOnDisplay) =>
            amountOfHoursOnDisplay * _plannerGridRowHeight;

        private List<Label> GeneratePlannerSideLabels(int earliestHourOnPlanningGrid, int latestHourOnPlanningGrid)
        {
            List<Label> plannerSideLabels = new List<Label>();
            for (int i = earliestHourOnPlanningGrid; i <= latestHourOnPlanningGrid; i++)
            {
                Label hourLabel = new Label { Content = $"{i}.00" };
                hourLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var margin = hourLabel.Margin;
                margin.Top = (i - earliestHourOnPlanningGrid) * _plannerGridRowHeight -
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
            for (int i = 0; i < _plannerGridWidth; i += _plannerGridColumnWidth)
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
                    X2 = _plannerGridWidth,
                    Y1 = (i - earliestHourOnPlanningGrid) * _plannerGridRowHeight,
                    Y2 = (i - earliestHourOnPlanningGrid) * _plannerGridRowHeight,
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
    }
}