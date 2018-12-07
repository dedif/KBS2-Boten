using System;
using System.Collections.Generic;
using System.Linq;
using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem.Reservations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace UnitTestProject2
{
    [TestClass]
    public class BoatTypeTabItemTests
    {
        [TestClass]
        public class GetEarliestSlot
        {
            [TestMethod]
            public void ShouldReturnHourIs12_WhenSunRiseIs1200()
            {
                // Arrange
                var now = DateTime.Now;
                var time1200 = new DateTime(now.Year, now.Month, now.Day, 12, 00, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                boats,
                new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstLightSlot(time1200);

                // Assert
                Assert.AreEqual(12, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs15_WhenSunRiseIs1200()
            {
                // Arrange
                var now = DateTime.Now;
                var time1200 = new DateTime(now.Year, now.Month, now.Day, 12, 00, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstLightSlot(time1200);

                // Assert
                Assert.AreEqual(15, actual.Minute);
            }

            [TestMethod]
            public void ShouldReturnHourIs12_WhenSunRiseIs1201()
            {
                // Arrange
                var now = DateTime.Now;
                var time1201 = new DateTime(now.Year, now.Month, now.Day, 12, 01, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstLightSlot(time1201);

                // Assert
                Assert.AreEqual(12, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs15_WhenSunRiseIs1201()
            {
                // Arrange
                var now = DateTime.Now;
                var time1201 = new DateTime(now.Year, now.Month, now.Day, 12, 01, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstLightSlot(time1201);

                // Assert
                Assert.AreEqual(15, actual.Minute);
            }

            [TestMethod]
            public void ShouldReturnHourIs12_WhenSunRiseIs1159()
            {
                // Arrange
                var now = DateTime.Now;
                var time1159 = new DateTime(now.Year, now.Month, now.Day, 11, 59, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstLightSlot(time1159);

                // Assert
                Assert.AreEqual(12, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs00_WhenSunRiseIs1159()
            {
                // Arrange
                var now = DateTime.Now;
                var time1159 = new DateTime(now.Year, now.Month, now.Day, 11, 59, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstLightSlot(time1159);

                // Assert
                Assert.AreEqual(00, actual.Minute);
            }
        }

        [TestClass]
        public class GetFirstDarknessSlot
        {
            [TestMethod]
            public void ShouldReturnHourIs12_WhenSunSetIs1200()
            {
                // Arrange
                var now = DateTime.Now;
                var time1200 = new DateTime(now.Year, now.Month, now.Day, 12, 00, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1200);

                // Assert
                Assert.AreEqual(12, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs00_WhenSunSetIs1200()
            {
                // Arrange
                var now = DateTime.Now;
                var time1200 = new DateTime(now.Year, now.Month, now.Day, 12, 00, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1200);

                // Assert
                Assert.AreEqual(00, actual.Minute);
            }

            [TestMethod]
            public void ShouldReturnHourIs12_WhenSunSetIs1201()
            {
                // Arrange
                var now = DateTime.Now;
                var time1201 = new DateTime(now.Year, now.Month, now.Day, 12, 01, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1201);

                // Assert
                Assert.AreEqual(12, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs00_WhenSunSetIs1201()
            {
                // Arrange
                var now = DateTime.Now;
                var time1201 = new DateTime(now.Year, now.Month, now.Day, 12, 01, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1201);

                // Assert
                Assert.AreEqual(00, actual.Minute);
            }

            [TestMethod]
            public void ShouldReturnHourIs11_WhenSunSetIs1159()
            {
                // Arrange
                var now = DateTime.Now;
                var time1159 = new DateTime(now.Year, now.Month, now.Day, 11, 59, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1159);

                // Assert
                Assert.AreEqual(11, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs45_WhenSunSetIs1159()
            {
                // Arrange
                var now = DateTime.Now;
                var time1159 = new DateTime(now.Year, now.Month, now.Day, 11, 59, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1159);

                // Assert
                Assert.AreEqual(45, actual.Minute);
            }

            [TestMethod]
            public void ShouldReturnHourIs12_WhenSunSetIs1215()
            {
                // Arrange
                var now = DateTime.Now;
                var time1215 = new DateTime(now.Year, now.Month, now.Day, 12, 15, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1215);

                // Assert
                Assert.AreEqual(12, actual.Hour);
            }

            [TestMethod]
            public void ShouldReturnMinuteIs15_WhenSunSetIs1215()
            {
                // Arrange
                var now = DateTime.Now;
                var time1215 = new DateTime(now.Year, now.Month, now.Day, 12, 15, 00);
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());

                // Act
                var actual = boatTypeTabItem.GetFirstDarknessSlot(time1215);

                // Assert
                Assert.AreEqual(15, actual.Minute);
            }
        }

        [TestClass]
        public class DateTimeToDayQuarter
        {
            private DateTime GetDateTimeWithGivenHoursAndMinutes(int hour, int minute)
            {
                var now = DateTime.Now;
                return new DateTime(now.Year, now.Month, now.Day, hour, minute, 00);
            }

            [TestMethod]
            public void ShouldReturn48_WhenTimeIs1200()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var time1200 = GetDateTimeWithGivenHoursAndMinutes(12, 00);

                // Act
                var actual = boatTypeTabItem.DateTimeToDayQuarter(time1200);

                // Assert
                Assert.AreEqual(48, actual);
            }

            [TestMethod]
            public void ShouldReturn00_WhenTimeIs0000()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var time0000 = GetDateTimeWithGivenHoursAndMinutes(00, 00);

                // Act
                var actual = boatTypeTabItem.DateTimeToDayQuarter(time0000);

                // Assert
                Assert.AreEqual(00, actual);
            }

            [TestMethod]
            public void ShouldReturn95_WhenTimeIs2345()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var time2345 = GetDateTimeWithGivenHoursAndMinutes(23, 45);

                // Act
                var actual = boatTypeTabItem.DateTimeToDayQuarter(time2345);

                // Assert
                Assert.AreEqual(95, actual);
            }

            [TestMethod]
            public void ShouldReturn95_WhenDateTimeIs2345()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var time2345 = GetDateTimeWithGivenHoursAndMinutes(23, 45);

                // Act
                var actual = boatTypeTabItem.DateTimeToDayQuarter(time2345);

                // Assert
                Assert.AreEqual(95, actual);
            }
        }

        [TestClass]
        public class DayQuarterToDateTime
        {
            private DateTime GetDateTimeWithGivenHoursAndMinutes(DateTime now, int hour, int minute) =>
                new DateTime(now.Year, now.Month, now.Day, hour, minute, 00);

            [TestMethod]
            public void ShouldReturn1200_WhenDayQuarterIs48()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var now = DateTime.Now;
                var dayQuarter48 = 48;

                // Act
                var actual = boatTypeTabItem.DayQuarterToDateTime(now, dayQuarter48);

                // Assert
                Assert.AreEqual(GetDateTimeWithGivenHoursAndMinutes(now, 12, 00), actual);
            }

            [TestMethod]
            public void ShouldReturn0000_WhenDayQuarterIs00()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var now = DateTime.Now;
                var dayQuarter00 = 00;

                // Act
                var actual = boatTypeTabItem.DayQuarterToDateTime(now, dayQuarter00);

                // Assert
                Assert.AreEqual(GetDateTimeWithGivenHoursAndMinutes(now, 00, 00), actual);
            }

            [TestMethod]
            public void ShouldReturn2345_WhenDayQuarterIs95()
            {
                // Arrange
                List<Boat> boats;
                using (var context = new DataBase()) boats = context.Boats.ToList();
                var boatTypeTabItem =
                    new BoatTypeTabItem(
                        boats,
                        new List<Reservation>());
                var now = DateTime.Now;
                var dayQuarter95 = 95;

                // Act
                var actual = boatTypeTabItem.DayQuarterToDateTime(now, dayQuarter95);

                // Assert
                Assert.AreEqual(GetDateTimeWithGivenHoursAndMinutes(now, 23, 45), actual);
            }
        }

        [TestClass]
        public class GetClaimedAndTooDistantSlots
        {
            private DateTime GetDateWithGivenMonthDayHourAndMinute(int month, int day, int hour, int minute)
            {
                return new DateTime(DateTime.Now.Year, month, day, hour, minute, 00);
            }

//            [TestMethod]
//            private void ShouldReturnAllClaimedSlotsAndSlotsFrom28February2016_0630_WhenNowIs26February0630()
//            {
//                // Arrange
//                List<Boat> boats;
//                using (var context = new DataBase()) boats = context.Boats.ToList();
//                var boatTypeTabItem =
//                    new BoatTypeTabItem(
//                        boats,
//                        new List<Reservation>());
//                var february28_2016_0630 = new DateTime(2016,2,28,6,30,00);
//                var february26_2016_0630 = new DateTime(2016,2,26,6,30,00);
//                var sunriseAndSunsetTimes = boatTypeTabItem.GetSunriseAndSunsetTimes(february26_2016_0630);
//                var earliestSlot = boatTypeTabItem.GetFirstLightSlot(sunriseAndSunsetTimes[0]);
//
//                var firstDarknessSlot = boatTypeTabItem.GetFirstDarknessSlot(sunriseAndSunsetTimes[1]);
//
//                // Act
//                boatTypeTabItem.GetClaimedAndTooDistantSlots(new List<DateTime>(), february28_2016_0630, )
//
//                // Assert
//            }
        }
    }
}