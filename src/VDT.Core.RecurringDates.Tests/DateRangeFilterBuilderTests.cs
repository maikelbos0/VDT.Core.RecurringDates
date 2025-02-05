using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateRangeFilterBuilderTests {
        [Fact]
        public void From() {
            var builder = new DateRangeFilterBuilder(new RecurrenceBuilder());

            Assert.Same(builder, builder.From(new DateTime(2022, 1, 1)));

            Assert.Equal(new DateTime(2022, 1, 1), builder.StartDate);
        }

        [Fact]
        public void Until() {
            var builder = new DateRangeFilterBuilder(new RecurrenceBuilder());

            Assert.Same(builder, builder.Until(new DateTime(2022, 12, 31)));

            Assert.Equal(new DateTime(2022, 12, 31), builder.EndDate);
        }

        [Fact]
        public void BuildFilter() {
            var builder = new DateRangeFilterBuilder(new RecurrenceBuilder()) {
                StartDate = new DateTime(2022, 2, 1),
                EndDate = new DateTime(2022, 3, 31)
            };

            var result = Assert.IsType<DateRangeFilter>(builder.BuildFilter());

            Assert.Equal(builder.StartDate, result.StartDate);
            Assert.Equal(builder.EndDate, result.EndDate);
        }
    }
}
