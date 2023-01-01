﻿using System;
using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class MonthlyRecurrencePatternTests {
        [Theory]
        [InlineData(1, "2022-12-01", "2022-12-01", true, 1)]
        [InlineData(1, "2022-12-01", "2022-11-01", true, 1)]
        [InlineData(1, "2022-12-01", "2022-12-01", true, 1, 3)]
        [InlineData(1, "2022-12-01", "2022-12-02", false, 1, 3)]
        [InlineData(1, "2022-12-01", "2022-12-03", true, 1, 3)]
        [InlineData(2, "2022-12-01", "2022-12-03", true, 3)]
        [InlineData(2, "2022-12-01", "2023-01-03", false, 3)]
        [InlineData(2, "2022-12-01", "2023-02-03", true, 3)]
        public void IsValid(int interval, DateTime referenceDate, DateTime date, bool expectedIsValid, params int[] daysOfMonth) {
            var pattern = new MonthlyRecurrencePattern(interval, referenceDate, daysOfMonth: daysOfMonth);

            Assert.Equal(expectedIsValid, pattern.IsValid(date));
        }

        [Theory]
        [InlineData(2022, 1, 1, 28, 29, 30, 31)]
        [InlineData(2022, 4, 1, 28, 29, 30)]
        [InlineData(2020, 2, 1, 28, 29)]
        [InlineData(2022, 2, 1, 28)]
        public void GetDaysOfMonth_DaysOfMonth(int year, int month, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfMonth: new int[] { 1, 28, 29, 30, 31 });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            Assert.Equal(expectedDays.ToHashSet(), result);
        }

        [Theory]
        [InlineData(2022, 1, 4, 12, 20, 28)]
        [InlineData(2022, 2, 1, 9, 17, 25)]
        [InlineData(2022, 4, 5, 13, 21, 22)]
        [InlineData(2022, 5, 3, 11, 19, 27)]
        public void GetDaysOfMonth_WeekDayOfMonth(int year, int month, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfWeek: new (DayOfWeekInMonth, DayOfWeek)[] {
                (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Second, DayOfWeek.Wednesday),
                (DayOfWeekInMonth.Third, DayOfWeek.Thursday),
                (DayOfWeekInMonth.Fourth, DayOfWeek.Friday)
            });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            Assert.Equal(expectedDays.ToHashSet(), result);
        }

        [Theory]
        [InlineData(2022, 1, 25, 26)]
        [InlineData(2022, 2, 22, 23)]
        [InlineData(2022, 3, 29, 30)]
        [InlineData(2022, 5, 25, 31)]
        public void GetDaysOfMonth_LastWeekDayOfMonth(int year, int month, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfWeek: new (DayOfWeekInMonth, DayOfWeek)[] {
                (DayOfWeekInMonth.Last, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Last, DayOfWeek.Wednesday)
            });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            Assert.Equal(expectedDays.ToHashSet(), result);
        }
    }
}
