using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class IRecurrenceBuilderDateOnlyExtensionsTests {
    [Fact]
    public void From() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.From(new DateOnly(2022, 1, 1));

        recurrenceBuilder.Received().From(new DateTime(2022, 1, 1));
    }

    [Fact]
    public void Until() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.Until(new DateOnly(2022, 12, 31));

        recurrenceBuilder.Received().Until(new DateTime(2022, 12, 31));
    }

    [Fact]
    public void ExceptOn_Array() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.ExceptOn(new DateOnly(2022, 1, 1), new DateOnly(2022, 1, 2));

        recurrenceBuilder.Received().ExceptOn(Arg.Is<IEnumerable<DateTime>>(dates => dates.SequenceEqual(new[] { new DateTime(2022, 1, 1), new DateTime(2022, 1, 2) })));
    }

    [Fact]
    public void ExceptOn_IEnumerable() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.ExceptOn(new List<DateOnly>() { new(2022, 1, 1), new(2022, 1, 2) });

        recurrenceBuilder.Received().ExceptOn(Arg.Is<IEnumerable<DateTime>>(dates => dates.SequenceEqual(new[] { new DateTime(2022, 1, 1), new DateTime(2022, 1, 2) })));
    }

    [Fact]
    public void ExceptFrom() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.ExceptFrom(new DateOnly(2022, 2, 3));

        recurrenceBuilder.Received().ExceptFrom(new DateTime(2022, 2, 3));
    }

    [Fact]
    public void ExceptUntil() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.ExceptUntil(new DateOnly(2022, 2, 5));

        recurrenceBuilder.Received().ExceptUntil(new DateTime(2022, 2, 5));
    }

    [Fact]
    public void ExceptBetween() {
        var recurrenceBuilder = Substitute.For<IRecurrenceBuilder>();

        recurrenceBuilder.ExceptBetween(new DateOnly(2022, 2, 3), new DateOnly(2022, 2, 5));

        recurrenceBuilder.Received().ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));
    }
}
