using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BataviaReseveringsSysteem.Reservations
{
    // Het reserveringstabelletje
    public class PlannerGrid : Canvas
    {
        private bool _selectsDuration;
        // Elke kolom is 50 pixels breed
        private const int PlannerGridColumnWidth = 50;

        // en 20 pixels hoog
        private const int PlannerGridRowHeight = 20;

        // De totale breedte is 200 pixels
        private const int PlannerGridWidth = 200;

        // Als een slot niet reserveerbaar is omdat het dan donker is,
        // dan is de kleur van dit slot donkerblauw
        private readonly Brush _slotDisabledDueToDarknessColor = new SolidColorBrush(Colors.MidnightBlue);

        // Als een slot al geclaimd is, of als hij in het verleden is,
        // of als hij te ver in de toekomst is,
        // dan is de kleur van dit slot grijs
        private readonly Brush _slotClaimedColor = new SolidColorBrush(Colors.Gray);

        // Als een slot geselecteerd wordt om te claimen,
        // dan wordt ie groen
        private readonly Brush _slotAboutToBeClaimedColor = new SolidColorBrush(Colors.Green);
        private int _earliestHourOnPlanningGrid;
        private const int LabelFontSize = 16;

        public PlannerGrid()
        {
            HorizontalAlignment = HorizontalAlignment.Right;
            VerticalAlignment = VerticalAlignment.Top;
            Width = PlannerGridWidth;

            // Schuif de tabel de grootte van een halve tekstlabel naar beneden
            var l = new Label { Content = "X", FontSize = LabelFontSize };
            l.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var margin = l.Margin;
            margin.Top = l.DesiredSize.Height;
            Margin = margin;
        }



        // Maak de horizontale lijnen 
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

        // Vul de plannergrid met alle lijnen, labels en slots die gemaakt moeten worden,
        // zonder groene slots
        public void Populate(DateTime earliestSlot, DateTime latestSlot, List<DateTime> claimedSlots)
        {
            _earliestHourOnPlanningGrid = GetEarliestHourOnPlanningGrid(earliestSlot);
            var latestHourOnPlanningGrid = GetLatestHourOnPlanningGrid(latestSlot);
            var amountOfHoursOnDisplay = latestHourOnPlanningGrid - _earliestHourOnPlanningGrid;

            // De hoogte van de grid is evenredig aan het aantal uren dat wordt weergegeven
            Height = GeneratePlannerGridHeight(amountOfHoursOnDisplay);

            Children.Clear();

            Children.Add(GenerateClickField(amountOfHoursOnDisplay));

            // Maak de labels aan de bovenkant
            GeneratePlannerSideLabels(_earliestHourOnPlanningGrid, latestHourOnPlanningGrid)
                .ForEach(plannerSideLabel => Children.Add(plannerSideLabel));

            var topLabels = GeneratePlannerTopLabels();
            topLabels.ForEach(plannerTopLabel => Children.Add(plannerTopLabel));

            // Schuif de applicatie naar links zodat de label rechtsboven niet over de tabview heen valt
            Margin = new Thickness(Margin.Left, Margin.Top, topLabels[topLabels.Count - 1].DesiredSize.Width / 2,
                Margin.Bottom);

            // Maak de horizontale lijnen die de rijen verdelen
            GeneratePlannerHorizontalLines(_earliestHourOnPlanningGrid, latestHourOnPlanningGrid)
                .ForEach(plannerHorizontalLine => Children.Add(plannerHorizontalLine));

            // Maak de verticale lijnen die de kolommen verdelen
            MakePlannerGridColumnDividers(
                amountOfHoursOnDisplay * PlannerGridRowHeight)
                .ForEach(plannerGridColumnDivider => Children.Add(plannerGridColumnDivider));

            // Maak de slots vóór zonsopgang blauw
            GetFirstUnavailableSlotTiles(earliestSlot, _earliestHourOnPlanningGrid)
                .ForEach(firstUnavailableSlotTile => Children.Add(firstUnavailableSlotTile));

            // Maak de slots na zonsopgang blauw
            GetLastUnavailableSlotTiles(latestSlot, _earliestHourOnPlanningGrid)
                .ForEach(lastUnavailableSlotTile => Children.Add(lastUnavailableSlotTile));

            // Maak de geclaimde slots grijs
            GetClaimedSlotTiles(claimedSlots, _earliestHourOnPlanningGrid)
                .ForEach(claimedSlotTile => Children.Add(claimedSlotTile));
        }

        private Rectangle GenerateClickField(int amountOfHoursOnDisplay) =>
            new Rectangle
            {
                Height = amountOfHoursOnDisplay * PlannerGridRowHeight,
                Width = PlannerGridWidth,
                Fill = new SolidColorBrush(Colors.Transparent)
            };

        // Vul de plannergrid met alle lijnen, labels en slots die gemaakt moeten worden,
        // met groene slots
        public void Populate(DateTime earliestSlot, DateTime latestSlot, List<DateTime> claimedSlots,
            List<DateTime> slotsAboutToBeClaimed)
        {
            // Vul de plannergrid met alle lijnen, labels en slots die gemaakt moeten worden,
            // zonder groene slots
            Populate(earliestSlot, latestSlot, claimedSlots);

            // Zet de groene slots erbij
            GetAboutToBeClaimedSlotTiles(slotsAboutToBeClaimed, _earliestHourOnPlanningGrid)
                .ForEach(slotAboutToBeClaimedTile => Children.Add(slotAboutToBeClaimedTile));
        }

        // Maak de slots die geselecteerd zijn groen
        private List<Rectangle> GetAboutToBeClaimedSlotTiles(List<DateTime> slotsAboutToBeClaimed, int firstHour) =>
            // Zet de DateTime-objecten van de slots om in grijze Rectangle-objecten
            slotsAboutToBeClaimed.ConvertAll(slotAboutToBeClaimed =>
                SlotAboutToBeClaimedTile(firstHour, slotAboutToBeClaimed.Hour, slotAboutToBeClaimed.Minute / 15));

        // Maak een groene slot
        private Rectangle SlotAboutToBeClaimedTile(int firstHour, int slotHour, int slotQuarter) =>
            OccupiedSlotTile(firstHour, slotHour, slotQuarter, _slotAboutToBeClaimedColor);

        // Maak de slots die geclaimd zijn grijs
        private List<Rectangle> GetClaimedSlotTiles(List<DateTime> claimedSlots, int firstHour) =>
            claimedSlots.ConvertAll(
                claimedSlot => ClaimedSlotTile(firstHour, claimedSlot.Hour, claimedSlot.Minute / 15));

        // Maak een grijze slot
        private int GeneratePlannerGridHeight(int amountOfHoursOnDisplay) =>
            amountOfHoursOnDisplay * PlannerGridRowHeight;

        // Maak de uurlabels aan de zijkant
        private List<Label> GeneratePlannerSideLabels(int earliestHourOnPlanningGrid, int latestHourOnPlanningGrid)
        {
            var plannerSideLabels = new List<Label>();
            for (var i = earliestHourOnPlanningGrid; i <= latestHourOnPlanningGrid; i++)
            {
                var hourLabel = new Label { Content = $"{i}.00", FontSize = LabelFontSize };

                // Zorg ervoor dat de label mooi is uitgelijnd
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

        // Maak de labels voor de kwartieren
        private List<Label> GeneratePlannerTopLabels()
        {
            var plannerTopLabels = new List<Label>();
            var minutes = 0;
            for (var i = 0; i <= PlannerGridWidth; i += PlannerGridColumnWidth)
            {
                var quarterHourLabel = new Label { Content = $"xx:{minutes}", FontSize = LabelFontSize };
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

        // Maak de horizontale lijnen die de rijen verdelen
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

        // Verkrijg het laatste uur dat op de plannergrid vermeld staat: altijd het uur na het uur dat de zon onder gaat
        private int GetLatestHourOnPlanningGrid(DateTime latestSlot) => latestSlot.Hour + 1;

        // Verkrijg het eerste uur dat op de plannergrid vermeld staat: altijd het uur na het uur dat de zon 
        private int GetEarliestHourOnPlanningGrid(DateTime earliestSlot) => earliestSlot.Hour;

        // Maak de slots na zonsopgang blauw
        private List<Rectangle> GetLastUnavailableSlotTiles(DateTime latestSlot, int firstHour)
        {
            var lastUnavailableSlotTiles = new List<Rectangle>();

            // Als de zon om 16.17 ondergaat,
            // dan moeten er 3 slots worden uitgeschakeld
            // 17 / 15 = 1 (niet 2,4666...)
            // 4 - 1 = 3
            var amountOfLastUnavailableSlots = 4 - latestSlot.Minute / 15;
            var latestHour = latestSlot.Hour;

            // Maak het aantal onclaimbare slots.
            // Begin daarbij bij de laatste slot
            // Daarom is de for-loop zo raar
            for (var i = 3; i > 3 - amountOfLastUnavailableSlots; i--)
                lastUnavailableSlotTiles.Add(SlotInDarknessTile(firstHour, latestHour, i));
            return lastUnavailableSlotTiles;
        }

        // Maak de slots vóór zonsopgang blauw
        private List<Rectangle> GetFirstUnavailableSlotTiles(DateTime earliestSlot, int firstHour)
        {
            var firstUnavailableSlotTiles = new List<Rectangle>();

            // Als de zon om 08.12 opgaat,
            // dan moet er 1 slot worden uitgeschakeld
            // 12 / 15 = 0 (niet 0,8)
            // 0 + 1 = 1
            var amountOfFirstUnavailableSlots = earliestSlot.Minute / 15 + 1;
            for (var i = 1; i < amountOfFirstUnavailableSlots; i++)
                firstUnavailableSlotTiles.Add(SlotInDarknessTile(firstHour, firstHour, i - 1));
            return firstUnavailableSlotTiles;
        }

        // Maak een donker slot
        private Rectangle SlotInDarknessTile(int firstHour, int disabledSlotHour, int quarter) =>
            OccupiedSlotTile(firstHour, disabledSlotHour, quarter, _slotDisabledDueToDarknessColor);

        // Maak een slot dat geclaimd is, in het verleden is of te ver weg is
        private Rectangle ClaimedSlotTile(int firstHour, int claimedSlotHour, int quarter) =>
            OccupiedSlotTile(firstHour, claimedSlotHour, quarter, _slotClaimedColor);

        // Maak een gekleurd slot
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

        public int GetHourFromY(double y) => (int)y / PlannerGridRowHeight + _earliestHourOnPlanningGrid;

        public int GetMinutesFromX(double x) => (int)x / PlannerGridColumnWidth * 15;
    }
}