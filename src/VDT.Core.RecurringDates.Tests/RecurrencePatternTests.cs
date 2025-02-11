using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class RecurrencePatternTests {
    private class TestRecurrencePattern : RecurrencePattern {
        public TestRecurrencePattern(int interval, DateTime? referenceDate) : base(interval, referenceDate) { }

        public override bool IsValid(DateTime date) => true;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Constructor_Throws_For_Invalid_Interval(int interval) {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TestRecurrencePattern(interval, null));
    }

    [Fact]
    public void Constructor_Without_ReferenceDate_Sets_DateTime_MinValue() {
        var pattern = new TestRecurrencePattern(1, null);

        Assert.Equal(DateTime.MinValue.Date, pattern.ReferenceDate);
    }

    [Fact]
    public void Constructor_Removes_Time_From_ReferenceDate() {
        var pattern = new TestRecurrencePattern(1, new DateTime(2022, 1, 2, 11, 12, 30));

        Assert.Equal(new DateTime(2022, 1, 2), pattern.ReferenceDate);
    }

    [Fact]
    public void IsValid_DateOnly() {
        var pattern = new TestRecurrencePattern(1, null);

        Assert.True(pattern.IsValid(new DateOnly(2022, 1, 2)));
    }
}
