using System;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class DateFilterBuilderTests {
    [Fact]
    public void On_Array_DateOnly() {
        var builder = new DateFilterBuilder(new RecurrenceBuilder()) {
            Dates = [
                new DateTime(2022, 2, 1),
                new DateTime(2022, 2, 3),
                new DateTime(2022, 2, 5)
            ]
        };

        Assert.Same(builder, builder.On(new DateOnly(2022, 2, 2), new DateOnly(2022, 2, 4)));

        Assert.Equal([
            new DateTime(2022, 2, 1),
            new DateTime(2022, 2, 3),
            new DateTime(2022, 2, 5),
            new DateTime(2022, 2, 2),
            new DateTime(2022, 2, 4)
        ], builder.Dates);
    }

    [Fact]
    public void On_IEnumerable_DateOnly() {
        var builder = new DateFilterBuilder(new RecurrenceBuilder()) {
            Dates = [
                new DateTime(2022, 2, 1),
                new DateTime(2022, 2, 3),
                new DateTime(2022, 2, 5)
            ]
        };

        Assert.Same(builder, builder.On(new List<DateOnly>() { new(2022, 2, 2), new(2022, 2, 4) }));

        Assert.Equal([
            new DateTime(2022, 2, 1),
            new DateTime(2022, 2, 3),
            new DateTime(2022, 2, 5),
            new DateTime(2022, 2, 2),
            new DateTime(2022, 2, 4)
        ], builder.Dates);
    }

    [Fact]
    public void On_Array_DateTime() {
        var builder = new DateFilterBuilder(new RecurrenceBuilder()) {
            Dates = [
                new DateTime(2022, 2, 1),
                new DateTime(2022, 2, 3),
                new DateTime(2022, 2, 5)
            ]
        };

        Assert.Same(builder, builder.On(new DateTime(2022, 2, 2), new DateTime(2022, 2, 4)));

        Assert.Equal([
            new DateTime(2022, 2, 1),
            new DateTime(2022, 2, 3),
            new DateTime(2022, 2, 5),
            new DateTime(2022, 2, 2),
            new DateTime(2022, 2, 4)
        ], builder.Dates);
    }

    [Fact]
    public void BuildFilter() {
        var builder = new DateFilterBuilder(new RecurrenceBuilder()) {
            Dates = [
                new DateTime(2022, 2, 1),
                new DateTime(2022, 2, 3),
                new DateTime(2022, 2, 5)
            ]
        };

        var result = Assert.IsType<DateFilter>(builder.BuildFilter());

        Assert.Equivalent(builder.Dates, result.Dates);
    }
}
