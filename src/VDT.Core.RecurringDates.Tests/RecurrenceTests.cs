using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrenceTests {
        private class TestRecurrencePattern : RecurrencePattern {
            public HashSet<DateTime> ValidDates { get; } = new();

            public TestRecurrencePattern(int interval, DateTime referenceDate) : base(interval, referenceDate) { }

            public override bool IsValid(DateTime date) => ValidDates.Contains(date);
        }

        [Fact]
        public void Constructor_Without_StartDate_Sets_DateTime_MinValue() {
            var recurrence = new Recurrence(null, new DateTime(2022, 1, 11), null, Enumerable.Empty<RecurrencePattern>(), false, Enumerable.Empty<IFilter>());

            Assert.Equal(DateTime.MinValue, recurrence.StartDate);
        }

        [Fact]
        public void Constructor_Without_EndDate_Sets_DateTime_MaxValue() {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), null, null, Enumerable.Empty<RecurrencePattern>(), false, Enumerable.Empty<IFilter>());

            Assert.Equal(DateTime.MaxValue, recurrence.EndDate);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void CacheDates(bool cacheDates, bool expectedCacheDates) {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 11), null, Enumerable.Empty<RecurrencePattern>(), cacheDates, Enumerable.Empty<IFilter>());

            Assert.Equal(expectedCacheDates, recurrence.CacheDates);
        }

        [Fact]
        public void GetDates() {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 4), null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            var dates = recurrence.GetDates();

            Assert.Equal(new[] {
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 3)
            }, dates);
        }

        [Fact]
        public void GetDates_From_To_Outside_StartDate_EndDate() {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 4), null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            var dates = recurrence.GetDates(DateTime.MinValue, DateTime.MaxValue);

            Assert.Equal(new[] {
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 3)
            }, dates);
        }

        [Fact]
        public void GetDates_From_To_Inside_StartDate_EndDate() {
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            var dates = recurrence.GetDates(new DateTime(2022, 1, 1), new DateTime(2022, 1, 4));

            Assert.Equal(new[] {
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 3)
            }, dates);
        }

        [Fact]
        public void GetDates_Occurrences() {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), DateTime.MaxValue, 2, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            var dates = recurrence.GetDates();

            Assert.Equal(new[] {
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 3)
            }, dates);
        }

        [Fact]
        public void GetDates_Occurrences_From_After_StartDate() {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), DateTime.MaxValue, 5, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            var dates = recurrence.GetDates(new DateTime(2022, 1, 6));

            Assert.Equal(new[] {
                new DateTime(2022, 1, 7),
                new DateTime(2022, 1, 9)
            }, dates);
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", false)]
        [InlineData("2022-01-05", true)]
        [InlineData("2022-01-06", false)]
        [InlineData("2022-01-07", false)]
        public void IsValid(DateTime date, bool expectedIsValid) {
            var recurrence = new Recurrence(new DateTime(2022, 1, 3), new DateTime(2022, 1, 6), null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 3)) }, false, Enumerable.Empty<IFilter>());

            Assert.Equal(expectedIsValid, recurrence.IsValid(date));
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", false)]
        [InlineData("2022-01-05", true)]
        [InlineData("2022-01-06", false)]
        [InlineData("2022-01-07", false)]
        public void IsValid_Occurrences(DateTime date, bool expectedIsValid) {
            var recurrence = new Recurrence(new DateTime(2022, 1, 3), DateTime.MaxValue, 2, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 3)) }, false, Enumerable.Empty<IFilter>());

            Assert.Equal(expectedIsValid, recurrence.IsValid(date));
        }

        [Theory]
        [InlineData("2022-01-01")]
        [InlineData("2022-01-02")]
        [InlineData("2022-01-03")]
        [InlineData("2022-01-04")]
        public void IsValidInPatternsAndFilters_No_Pattern(DateTime date) {
            var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 11), null, Enumerable.Empty<RecurrencePattern>(), false, Enumerable.Empty<IFilter>());

            Assert.False(recurrence.IsValidInPatternsAndFilters(date));
        }

        [Theory]
        [InlineData("2022-01-01", true)]
        [InlineData("2022-01-02", false)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", false)]
        public void IsValidInPatternsAndFilters_Single_Pattern(DateTime date, bool expectedIsValid) {
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-02", true)]
        [InlineData("2022-01-03", false)]
        [InlineData("2022-01-04", true)]
        public void IsValidInPatternsAndFilters_Offset_ReferenceDate(DateTime date, bool expectedIsValid) {
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 2)) }, false, Enumerable.Empty<IFilter>());

            Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
        }

        [Theory]
        [InlineData("2022-01-01", true)]
        [InlineData("2022-01-02", false)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", true)]
        public void IsValidInPatternsAndFilters_Double_Pattern(DateTime date, bool expectedIsValid) {
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)), new DailyRecurrencePattern(3, new DateTime(2022, 1, 1)) }, false, Enumerable.Empty<IFilter>());

            Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-02", true)]
        [InlineData("2022-01-03", false)]
        [InlineData("2022-01-04", true)]
        public void IsValidInPatternsAndFilters_Filters(DateTime date, bool expectedIsValid) {
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { new DailyRecurrencePattern(1, new DateTime(2022, 1, 1)), }, false, new[] { new DateFilter(new DateTime(2022, 1, 1)), new DateFilter(new DateTime(2022, 1, 3)) });

            Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
        }

        [Fact]
        public void IsValidInPatternsAndFilters_Caches_When_CacheDates_Is_True() {
            var recurrencePattern = new TestRecurrencePattern(2, new DateTime(2022, 1, 1));
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { recurrencePattern }, true, Enumerable.Empty<IFilter>());

            var firstResult = recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1));

            recurrencePattern.ValidDates.Add(new DateTime(2022, 1, 1));

            Assert.Equal(firstResult, recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1)));
        }

        [Fact]
        public void IsValidInPatternsAndFilters_Does_Not_Cache_When_CacheDates_Is_False() {
            var recurrencePattern = new TestRecurrencePattern(2, new DateTime(2022, 1, 1));
            var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, new[] { recurrencePattern }, false, Enumerable.Empty<IFilter>());

            var firstResult = recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1));

            recurrencePattern.ValidDates.Add(new DateTime(2022, 1, 1));

            Assert.NotEqual(firstResult, recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1)));
        }
    }
}
