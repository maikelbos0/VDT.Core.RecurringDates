using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrencePatternBuilderStartTests {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Constructor_Throws_For_Invalid_Interval(int interval) {
            var action = () => new RecurrencePatternBuilderStart(new RecurrenceBuilder(), interval);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Days() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var start = new RecurrencePatternBuilderStart(recurrenceBuilder, 2);

            var result = start.Days();

            result.RecurrenceBuilder.Should().BeSameAs(recurrenceBuilder);
            recurrenceBuilder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(2);
        }

        [Fact]
        public void Weeks() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var start = new RecurrencePatternBuilderStart(recurrenceBuilder, 2);

            var result = start.Weeks();

            result.RecurrenceBuilder.Should().BeSameAs(recurrenceBuilder);
            recurrenceBuilder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(2);
        }

        [Fact]
        public void Months() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var start = new RecurrencePatternBuilderStart(recurrenceBuilder, 2);

            var result = start.Months();

            result.RecurrenceBuilder.Should().BeSameAs(recurrenceBuilder);
            recurrenceBuilder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(2);
        }
    }
}
