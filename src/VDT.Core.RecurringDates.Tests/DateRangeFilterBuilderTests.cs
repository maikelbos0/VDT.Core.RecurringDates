using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateRangeFilterBuilderTests {
        [Fact]
        public void FilterFrom() {
            var builder = new DateRangeFilterBuilder(new RecurrenceBuilder());

            builder.Should().BeSameAs(builder.FilterFrom(new DateTime(2022, 1, 1)));

            builder.StartDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void FilterUntil() {
            var builder = new DateRangeFilterBuilder(new RecurrenceBuilder());

            builder.Should().BeSameAs(builder.FilterUntil(new DateTime(2022, 12, 31)));

            builder.EndDate.Should().Be(new DateTime(2022, 12, 31));
        }

        [Fact]
        public void BuildFilter() {
            var builder = new DateRangeFilterBuilder(new RecurrenceBuilder()) {
                StartDate = new DateTime(2022, 2, 1),
                EndDate = new DateTime(2022, 3, 31)
            };

            var result = builder.BuildFilter().Should().BeOfType<DateRangeFilter>().Subject;

            result.StartDate.Should().Be(builder.StartDate);
            result.EndDate.Should().Be(builder.EndDate);
        }
    }
}
