using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class IFilterDateOnlyExtensionsTests {
    [Fact]
    public void From() {
        var filter = Substitute.For<IFilter>();

        filter.IsFiltered(new DateOnly(2022, 1, 1));

        filter.Received().IsFiltered(new DateTime(2022, 1, 1));
    }
}
