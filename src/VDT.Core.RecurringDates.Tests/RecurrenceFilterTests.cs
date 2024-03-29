﻿using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrenceFilterTests {
        [Theory]
        [InlineData("2023-04-12", false)]
        [InlineData("2023-04-13", false)]
        [InlineData("2023-04-14", false)]
        [InlineData("2023-04-15", true)]
        [InlineData("2023-04-16", true)]
        [InlineData("2023-04-17", false)]
        [InlineData("2023-04-18", false)]
        public void IsFiltered(DateTime date, bool expectedIsFiltered) {
            var filter = new RecurrenceFilter(new Recurrence(
                null,
                null,
                null,
                new[] { new WeeklyRecurrencePattern(1, null, null, new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }) },
                Enumerable.Empty<IFilter>(),
                false
            ));

            filter.IsFiltered(date).Should().Be(expectedIsFiltered);
        }
    }
}
