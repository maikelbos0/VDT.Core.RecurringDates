using System;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class WeeklyRecurrencePatternBuilderTests {
    [Fact]
    public void UsingFirstDayOfWeek() {
        var builder = new WeeklyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

        Assert.Same(builder, builder.UsingFirstDayOfWeek(DayOfWeek.Tuesday));

        Assert.Equal(DayOfWeek.Tuesday, builder.FirstDayOfWeek);
    }

    [Fact]
    public void On() {
        var builder = new WeeklyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
            DaysOfWeek = [
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday
            ]
        };

        Assert.Same(builder, builder.On(DayOfWeek.Friday, DayOfWeek.Monday));

        Assert.Equivalent(new List<DayOfWeek>() {
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Monday
        }, builder.DaysOfWeek);
    }

    [Fact]
    public void BuildPattern() {
        var recurrenceBuilder = new RecurrenceBuilder();
        var builder = new WeeklyRecurrencePatternBuilder(recurrenceBuilder, 2) {
            ReferenceDate = new DateTime(2022, 2, 1),
            FirstDayOfWeek = DayOfWeek.Wednesday,
            DaysOfWeek = [DayOfWeek.Sunday, DayOfWeek.Thursday]
        };

        var result = Assert.IsType<WeeklyRecurrencePattern>(builder.BuildPattern());

        Assert.Equal(builder.ReferenceDate, result.ReferenceDate);
        Assert.Equal(builder.Interval, result.Interval);
        Assert.Equal(builder.FirstDayOfWeek, result.FirstDayOfWeek);
        Assert.Equivalent(builder.DaysOfWeek, result.DaysOfWeek);
    }

    [Fact]
    public void BuildPattern_Takes_StartDate_As_Default_ReferenceDate() {
        var recurrenceBuilder = new RecurrenceBuilder() { StartDate = new DateTime(2022, 2, 1) };
        var builder = new WeeklyRecurrencePatternBuilder(recurrenceBuilder, 2);

        var result = builder.BuildPattern();

        Assert.Equal(recurrenceBuilder.StartDate, result.ReferenceDate);
    }
}
