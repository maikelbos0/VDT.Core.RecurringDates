﻿using System;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class RecurrenceTests {
    private class TestRecurrencePattern : RecurrencePattern {
        public HashSet<DateTime> ValidDates { get; } = [];

        public TestRecurrencePattern(int interval, DateTime referenceDate) : base(interval, referenceDate) { }

        public override bool IsValid(DateTime date) => ValidDates.Contains(date);
    }

    [Fact]
    public void Constructor_Without_StartDate_Sets_DateTime_MinValue() {
        var recurrence = new Recurrence(null, new DateTime(2022, 1, 11), null, [], [], false);

        Assert.Equal(DateTime.MinValue.Date, recurrence.StartDate);
    }

    [Fact]
    public void Constructor_Removes_Time_From_StartDate() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 2, 11, 12, 30), new DateTime(2022, 1, 3, 12, 37, 30), null, [], [], false);

        Assert.Equal(new DateTime(2022, 1, 2), recurrence.StartDate);
    }

    [Fact]
    public void Constructor_Without_EndDate_Sets_DateTime_MaxValue() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), null, null, [], [], false);

        Assert.Equal(DateTime.MaxValue.Date, recurrence.EndDate);
    }

    [Fact]
    public void Constructor_Removes_Time_From_EndDate() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 2, 11, 12, 30), new DateTime(2022, 1, 3, 12, 37, 30), null, [], [], false);

        Assert.Equal(new DateTime(2022, 1, 3), recurrence.EndDate);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void CacheDates(bool cacheDates, bool expectedCacheDates) {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 11), null, [], [], cacheDates);

        Assert.Equal(expectedCacheDates, recurrence.CacheDates);
    }

    [Fact]
    public void GetDates() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 5), null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates((DateTime?)null, null);

        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 3), new DateTime(2022, 1, 5)], dates);
    }

    [Fact]
    public void GetDates_Removes_Time_From_From() {
        var recurrence = new Recurrence(null, new DateTime(2022, 1, 4), null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates(new DateTime(2022, 1, 1, 11, 12, 30));

        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 3)], dates);
    }

    [Fact]
    public void GetDates_Until_DateTime_MaxValue() {
        var recurrence = new Recurrence((DateOnly?)null, null, null, [new DailyRecurrencePattern(1, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates(new DateTime(9999, 12, 30), DateTime.MaxValue);

        Assert.Equal([new DateTime(9999, 12, 30), new DateTime(9999, 12, 31)], dates);
    }

    [Fact]
    public void GetDates_Until_DateTime_MaxValue_With_Occurrences() {
        var recurrence = new Recurrence(new DateTime(9999, 12, 30), null, 5, [new DailyRecurrencePattern(1, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates(new DateTime(9999, 12, 30), DateTime.MaxValue);

        Assert.Equal([new DateTime(9999, 12, 30), new DateTime(9999, 12, 31)], dates);
    }

    [Fact]
    public void GetDates_From_To_Outside_StartDate_EndDate() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 4), null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates(DateTime.MinValue, DateTime.MaxValue);

        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 3)], dates);
    }

    [Fact]
    public void GetDates_From_To_Inside_StartDate_EndDate() {
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates(new DateTime(2022, 1, 1), new DateTime(2022, 1, 4));

        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 3)], dates);
    }

    [Fact]
    public void GetDates_Occurrences() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), DateTime.MaxValue, 2, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates((DateTime?)null, null);

        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 3)], dates);
    }

    [Fact]
    public void GetDates_Occurrences_From_After_StartDate() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), DateTime.MaxValue, 5, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        var dates = recurrence.GetDates(new DateTime(2022, 1, 6));

        Assert.Equal([new DateTime(2022, 1, 7), new DateTime(2022, 1, 9)], dates);
    }

    [Theory]
    [InlineData("2022-01-01", false)]
    [InlineData("2022-01-02", false)]
    [InlineData("2022-01-03", true)]
    [InlineData("2022-01-04", false)]
    [InlineData("2022-01-05", true)]
    [InlineData("2022-01-06", false)]
    [InlineData("2022-01-07", false)]
    public void IsValid(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(new DateTime(2022, 1, 3), new DateTime(2022, 1, 6), null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 3))], [], false);

        Assert.Equal(expectedIsValid, recurrence.IsValid(date));
    }

    [Fact]
    public void IsValid_Removes_Time_From_Date() {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 4), null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        Assert.True(recurrence.IsValid(new DateTime(2022, 1, 1, 11, 12, 30)));
    }

    [Theory]
    [InlineData("2022-01-01", false)]
    [InlineData("2022-01-02", false)]
    [InlineData("2022-01-03", true)]
    [InlineData("2022-01-04", false)]
    [InlineData("2022-01-05", true)]
    [InlineData("2022-01-06", false)]
    [InlineData("2022-01-07", false)]
    public void IsValid_Occurrences(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(new DateTime(2022, 1, 3), DateTime.MaxValue, 2, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 3))], [], false);

        Assert.Equal(expectedIsValid, recurrence.IsValid(date));
    }

    [Theory]
    [InlineData("2022-01-01")]
    [InlineData("2022-01-02")]
    [InlineData("2022-01-03")]
    [InlineData("2022-01-04")]
    public void IsValidInPatternsAndFilters_No_Pattern(DateTime date) {
        var recurrence = new Recurrence(new DateTime(2022, 1, 1), new DateTime(2022, 1, 11), null, [], [], false);

        Assert.False(recurrence.IsValidInPatternsAndFilters(date));
    }

    [Theory]
    [InlineData("2022-01-01", true)]
    [InlineData("2022-01-02", false)]
    [InlineData("2022-01-03", true)]
    [InlineData("2022-01-04", false)]
    public void IsValidInPatternsAndFilters_Single_Pattern(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1))], [], false);

        Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
    }

    [Theory]
    [InlineData("2022-01-01", false)]
    [InlineData("2022-01-02", true)]
    [InlineData("2022-01-03", false)]
    [InlineData("2022-01-04", true)]
    public void IsValidInPatternsAndFilters_Offset_ReferenceDate(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 2))], [], false);

        Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
    }

    [Theory]
    [InlineData("2022-01-01", true)]
    [InlineData("2022-01-02", false)]
    [InlineData("2022-01-03", true)]
    [InlineData("2022-01-04", true)]
    public void IsValidInPatternsAndFilters_Double_Pattern(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [new DailyRecurrencePattern(2, new DateTime(2022, 1, 1)), new DailyRecurrencePattern(3, new DateTime(2022, 1, 1))], [], false);

        Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
    }

    [Theory]
    [InlineData("2022-01-01", false)]
    [InlineData("2022-01-02", true)]
    [InlineData("2022-01-03", false)]
    [InlineData("2022-01-04", true)]
    public void IsValidInPatternsAndFilters_Single_Filter(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [new DailyRecurrencePattern(1, new DateTime(2022, 1, 1)),], [new DateFilter([new DateTime(2022, 1, 1), new DateTime(2022, 1, 3)])], false);

        Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
    }

    [Theory]
    [InlineData("2022-01-01", false)]
    [InlineData("2022-01-02", true)]
    [InlineData("2022-01-03", false)]
    [InlineData("2022-01-04", true)]
    public void IsValidInPatternsAndFilters_Double_Filter(DateTime date, bool expectedIsValid) {
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [new DailyRecurrencePattern(1, new DateTime(2022, 1, 1)),], [new DateFilter([new DateTime(2022, 1, 1)]), new DateFilter([new DateTime(2022, 1, 3)])], false);

        Assert.Equal(expectedIsValid, recurrence.IsValidInPatternsAndFilters(date));
    }

    [Fact]
    public void IsValidInPatternsAndFilters_Caches_When_CacheDates_Is_True() {
        var recurrencePattern = new TestRecurrencePattern(2, new DateTime(2022, 1, 1));
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [recurrencePattern], [], true);

        var firstResult = recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1));

        recurrencePattern.ValidDates.Add(new DateTime(2022, 1, 1));

        Assert.Equal(firstResult, recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1)));
    }

    [Fact]
    public void IsValidInPatternsAndFilters_Does_Not_Cache_When_CacheDates_Is_False() {
        var recurrencePattern = new TestRecurrencePattern(2, new DateTime(2022, 1, 1));
        var recurrence = new Recurrence(DateTime.MinValue, DateTime.MaxValue, null, [recurrencePattern], [], false);

        var firstResult = recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1));

        recurrencePattern.ValidDates.Add(new DateTime(2022, 1, 1));

        Assert.NotEqual(firstResult, recurrence.IsValidInPatternsAndFilters(new DateTime(2022, 1, 1)));
    }
}
