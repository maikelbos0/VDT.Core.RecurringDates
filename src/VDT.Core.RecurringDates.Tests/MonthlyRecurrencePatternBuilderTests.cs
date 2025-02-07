using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class MonthlyRecurrencePatternBuilderTests {
    [Fact]
    public void On_DaysOfMonth() {
        var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
            DaysOfMonth = [5, 9, 17]
        };

        Assert.Same(builder, builder.On(11, 19));

        Assert.Equal([5, 9, 17, 11, 19], builder.DaysOfMonth);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(9, 19, -1)]
    [InlineData(int.MinValue)]
    public void On_DaysOfMonth_Throws_For_Invalid_Days(params int[] days) {
        var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.On(days));
    }

    [Fact]
    public void On_DayOfWeek() {
        var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
            DaysOfWeek = [
                (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Third, DayOfWeek.Friday),
                (DayOfWeekInMonth.First, DayOfWeek.Monday)
            ]
        };

        Assert.Same(builder, builder.On(DayOfWeekInMonth.Third, DayOfWeek.Thursday));

        Assert.Equal([
            (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
            (DayOfWeekInMonth.Third, DayOfWeek.Friday),
            (DayOfWeekInMonth.First, DayOfWeek.Monday),
            (DayOfWeekInMonth.Third, DayOfWeek.Thursday)
        ], builder.DaysOfWeek);
    }

    [Fact]
    public void On_DaysOfWeek() {
        var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
            DaysOfWeek = [
                (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Third, DayOfWeek.Friday),
                (DayOfWeekInMonth.First, DayOfWeek.Monday)
            ]
        };

        Assert.Same(builder, builder.On((DayOfWeekInMonth.Second, DayOfWeek.Friday), (DayOfWeekInMonth.Third, DayOfWeek.Thursday)));

        Assert.Equal([
            (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
            (DayOfWeekInMonth.Third, DayOfWeek.Friday),
            (DayOfWeekInMonth.First, DayOfWeek.Monday),
            (DayOfWeekInMonth.Second, DayOfWeek.Friday),
            (DayOfWeekInMonth.Third, DayOfWeek.Thursday)
        ], builder.DaysOfWeek);
    }

    [Fact]
    public void On_LastDaysOfMonth() {
        var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
            LastDaysOfMonth = [
                LastDayOfMonth.Last,
                LastDayOfMonth.FourthLast
            ]
        };

        Assert.Same(builder, builder.On(LastDayOfMonth.SecondLast, LastDayOfMonth.FifthLast));

        Assert.Equal([
            LastDayOfMonth.Last,
            LastDayOfMonth.FourthLast,
            LastDayOfMonth.SecondLast,
            LastDayOfMonth.FifthLast
        ], builder.LastDaysOfMonth);
    }

    [Fact]
    public void WithDaysOfMonthCaching() {
        var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

        Assert.Same(builder, builder.WithDaysOfMonthCaching());

        Assert.True(builder.CacheDaysOfMonth);
    }

    [Fact]
    public void BuildPattern() {
        var recurrenceBuilder = new RecurrenceBuilder();
        var builder = new MonthlyRecurrencePatternBuilder(recurrenceBuilder, 2) {
            ReferenceDate = new DateTime(2022, 2, 1),
            DaysOfMonth = [3, 5, 20],
            DaysOfWeek = [(DayOfWeekInMonth.Third, DayOfWeek.Thursday), (DayOfWeekInMonth.First, DayOfWeek.Sunday)],
            LastDaysOfMonth = [LastDayOfMonth.Last, LastDayOfMonth.FourthLast],
            CacheDaysOfMonth = true
        };

        var result = Assert.IsType<MonthlyRecurrencePattern>(builder.BuildPattern());

        Assert.Equal(builder.ReferenceDate, result.ReferenceDate);
        Assert.Equal(builder.Interval, result.Interval);
        Assert.Equivalent(builder.DaysOfMonth, result.DaysOfMonth);
        Assert.Equivalent(builder.DaysOfWeek, result.DaysOfWeek);
        Assert.Equivalent(builder.LastDaysOfMonth, result.LastDaysOfMonth);
        Assert.Equal(builder.CacheDaysOfMonth, result.CacheDaysOfMonth);
    }

    [Fact]
    public void BuildPattern_Takes_StartDate_As_Default_ReferenceDate() {
        var recurrenceBuilder = new RecurrenceBuilder() { StartDate = new DateTime(2022, 2, 1) };
        var builder = new MonthlyRecurrencePatternBuilder(recurrenceBuilder, 2);

        var result = builder.BuildPattern();

        Assert.Equal(recurrenceBuilder.StartDate, result.ReferenceDate);
    }
}
