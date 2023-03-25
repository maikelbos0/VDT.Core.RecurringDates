using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecursTests {
        [Fact]
        public void From() {
            var result = Recurs.From(new DateTime(2022, 1, 1));

            result.Should().BeOfType<RecurrenceBuilder>().Which.StartDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void Until() {
            var result = Recurs.Until(new DateTime(2022, 12, 31));

            result.Should().BeOfType<RecurrenceBuilder>().Which.EndDate.Should().Be(new DateTime(2022, 12, 31));
        }

        [Fact]
        public void StopAfter() {
            var result = Recurs.StopAfter(10);

            result.Should().BeOfType<RecurrenceBuilder>().Which.Occurrences.Should().Be(10);
        }

        [Fact]
        public void Daily() {
            var result = Recurs.Daily();

            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Weekly() {
            var result = Recurs.Weekly();

            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Monthly() {
            var result = Recurs.Monthly();

            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Every() {
            var result = Recurs.Every(2);

            result.Interval.Should().Be(2);
        }

        [Fact]
        public void WithDateCaching() {
            var result = Recurs.WithDateCaching();

            result.CacheDates.Should().BeTrue();
        }
    }
}
