﻿using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class MonthlyRecurrencePatternTests {
        [Fact]
        public void Constructor_Without_Days_Adds_Default_Day() {
            var startDate = new DateTime(2022, 1, 15);
            var pattern = new MonthlyRecurrencePattern(1, startDate);

            pattern.DaysOfMonth.Should().ContainSingle().Which.Should().Be(startDate.Day);
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(9, 19, -1)]
        [InlineData(int.MinValue)]
        public void Constructor_Throws_For_Invalid_DaysOfMonth(params int[] daysOfMonth) {
            var action = () => new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfMonth: daysOfMonth);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void CacheDaysOfMonth(bool cacheDaysOfMonth, bool expectedCacheDaysOfMonth) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, cacheDaysOfMonth: cacheDaysOfMonth);

            pattern.CacheDaysOfMonth.Should().Be(expectedCacheDaysOfMonth);
        }

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

            pattern.IsValid(date).Should().Be(expectedIsValid);
        }

        [Theory]
        [InlineData(2022, 1, 1, 28, 29, 30, 31)]
        [InlineData(2022, 4, 1, 28, 29, 30)]
        [InlineData(2020, 2, 1, 28, 29)]
        [InlineData(2022, 2, 1, 28)]
        public void GetDaysOfMonth_DaysOfMonth(int year, int month, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfMonth: new[] { 1, 28, 29, 30, 31 });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            result.Should().BeEquivalentTo(expectedDays);
        }

        [Theory]
        [InlineData(2022, 1, 4, 12, 20, 28)]
        [InlineData(2022, 2, 1, 9, 17, 25)]
        [InlineData(2022, 4, 5, 13, 21, 22)]
        [InlineData(2022, 5, 3, 11, 19, 27)]
        public void GetDaysOfMonth_WeekDayOfMonth(int year, int month, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfWeek: new[] {
                (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Second, DayOfWeek.Wednesday),
                (DayOfWeekInMonth.Third, DayOfWeek.Thursday),
                (DayOfWeekInMonth.Fourth, DayOfWeek.Friday)
            });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            result.Should().BeEquivalentTo(expectedDays);
        }

        [Theory]
        [InlineData(2022, 1, 25, 26)]
        [InlineData(2022, 2, 22, 23)]
        [InlineData(2022, 3, 29, 30)]
        [InlineData(2022, 5, 25, 31)]
        public void GetDaysOfMonth_LastWeekDayOfMonth(int year, int month, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, daysOfWeek: new[] {
                (DayOfWeekInMonth.Last, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Last, DayOfWeek.Wednesday)
            });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            result.Should().BeEquivalentTo(expectedDays);
        }

        [Theory]
        [InlineData(2022, 1, LastDayOfMonth.Last, 31)]
        [InlineData(2022, 4, LastDayOfMonth.SecondLast, 29)]
        [InlineData(2020, 2, LastDayOfMonth.ThirdLast, 27)]
        [InlineData(2022, 2, LastDayOfMonth.FourthLast, 25)]
        [InlineData(2022, 2, LastDayOfMonth.FifthLast, 24)]
        public void GetDaysOfMonth_LastDayOfMonth(int year, int month, LastDayOfMonth lastDayOfMonth, params int[] expectedDays) {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, lastDaysOfMonth: new[] { lastDayOfMonth });

            var result = pattern.GetDaysOfMonth(new DateTime(year, month, 1));

            result.Should().BeEquivalentTo(expectedDays);
        }

        [Fact]
        public void GetDaysOfMonth_LastDaysOfMonth() {
            var pattern = new MonthlyRecurrencePattern(1, DateTime.MinValue, lastDaysOfMonth: new[] { LastDayOfMonth.FifthLast, LastDayOfMonth.ThirdLast, LastDayOfMonth.Last });

            var result = pattern.GetDaysOfMonth(new DateTime(2022, 1, 1));

            result.Should().BeEquivalentTo(new[] { 27, 29, 31 });
        }

        [Fact]
        public void GetDaysOfMonth_Caches_When_CacheDaysOfMonth_Is_True() {
            var pattern = new MonthlyRecurrencePattern(
                1,
                DateTime.MinValue,
                daysOfMonth: new[] { 5, 10 },
                daysOfWeek: new[] { (DayOfWeekInMonth.Third, DayOfWeek.Thursday) },
                lastDaysOfMonth: new[] { LastDayOfMonth.Last },
                true
            );

            pattern.GetDaysOfMonth(new DateTime(2022, 1, 1)).Should().BeSameAs(pattern.GetDaysOfMonth(new DateTime(2022, 1, 1)));
        }

        [Fact]
        public void GetDaysOfMonth_Caches_When_CacheDaysOfMonth_Is_False() {
            var pattern = new MonthlyRecurrencePattern(
                1,
                DateTime.MinValue,
                daysOfMonth: new[] { 5, 10 },
                daysOfWeek: new[] { (DayOfWeekInMonth.Third, DayOfWeek.Thursday) },
                lastDaysOfMonth: new[] { LastDayOfMonth.Last },
                false
            );

            pattern.GetDaysOfMonth(new DateTime(2022, 1, 1)).Should().NotBeSameAs(pattern.GetDaysOfMonth(new DateTime(2022, 1, 1)));
        }
    }
}
