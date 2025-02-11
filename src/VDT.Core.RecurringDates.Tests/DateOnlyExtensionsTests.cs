using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class DateOnlyExtensionsTests {
    [Fact]
    public void ToDateTime() {
        Assert.Equal(new DateTime(2022, 4, 15), new DateOnly(2022, 4, 15).ToDateTime());
    }
}
