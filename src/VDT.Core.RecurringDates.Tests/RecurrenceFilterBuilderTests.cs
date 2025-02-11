using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class RecurrenceFilterBuilderTests {
    [Fact]
    public void Intersecting() {
        var recurrence = new Recurrence((DateOnly?)null, null, null, [], [], false);
        var builder = new RecurrenceFilterBuilder(new RecurrenceBuilder(), new Recurrence((DateOnly?)null, null, null, [], [], false));

        Assert.Same(builder, builder.Intersecting(recurrence));

        Assert.Same(recurrence, builder.Recurrence);
    }

    [Fact]
    public void BuildFilter() {
        var recurrence = new Recurrence((DateOnly?)null, null, null, [], [], false);
        var builder = new RecurrenceFilterBuilder(new RecurrenceBuilder(), recurrence);

        var result = Assert.IsType<RecurrenceFilter>(builder.BuildFilter());

        Assert.Same(recurrence, result.Recurrence);
    }
}
