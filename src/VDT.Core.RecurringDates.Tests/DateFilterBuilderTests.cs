﻿using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateFilterBuilderTests {
        [Fact]
        public void On() {
            var builder = new DateFilterBuilder(new RecurrenceBuilder()) {
                Dates = new() {
                    new DateTime(2022, 2, 1),
                    new DateTime(2022, 2, 3),
                    new DateTime(2022, 2, 5)
                }
            };

            builder.Should().BeSameAs(builder.On(new DateTime(2022, 2, 2), new DateTime(2022, 2, 4)));

            builder.Dates.Should().Equal(
                new DateTime(2022, 2, 1),
                new DateTime(2022, 2, 3),
                new DateTime(2022, 2, 5),
                new DateTime(2022, 2, 2),
                new DateTime(2022, 2, 4)
            );
        }

        [Fact]
        public void BuildFilter() {
            var builder = new DateFilterBuilder(new RecurrenceBuilder()) {
                Dates = new() {
                    new DateTime(2022, 2, 1),
                    new DateTime(2022, 2, 3),
                    new DateTime(2022, 2, 5)
                }
            };

            builder.BuildFilter().Should().BeOfType<DateFilter>().Which.Dates.Should().BeEquivalentTo(builder.Dates);
        }
    }
}
