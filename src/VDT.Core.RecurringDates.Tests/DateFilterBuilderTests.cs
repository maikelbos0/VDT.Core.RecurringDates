using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class DateFilterBuilderTests {
    [Fact]
    public void On() {
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
