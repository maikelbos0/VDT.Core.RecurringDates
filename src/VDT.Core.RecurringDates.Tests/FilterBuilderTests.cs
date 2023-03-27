using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class FilterBuilderTests {
        private class TestFilterBuilder : FilterBuilder {
            public TestFilterBuilder(RecurrenceBuilder recurrenceBuilder) : base(recurrenceBuilder) { }

            public override IFilter BuildFilter() => throw new NotImplementedException();
        }

        [Fact]
        public void From() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(filterBuilder.From(new DateTime(2022, 1, 1)));

            builder.StartDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void Until() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(filterBuilder.Until(new DateTime(2022, 12, 31)));

            builder.EndDate.Should().Be(new DateTime(2022, 12, 31));
        }

        [Fact]
        public void StopAfter() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(filterBuilder.StopAfter(10));

            builder.Occurrences.Should().Be(10);
        }

        [Fact]
        public void Daily() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Daily();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Weekly() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Weekly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Monthly() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Monthly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Every() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Every(2);

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.Interval.Should().Be(2);
        }

        [Fact]
        public void WithDateCaching() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(filterBuilder.WithDateCaching());

            builder.CacheDates.Should().BeTrue();
        }

        [Fact]
        public void ExceptOn() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptOn(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.Dates.Should().Equal(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));
        }

        [Fact]
        public void ExceptStartingOn() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptStartingOn(new DateTime(2022, 2, 3));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.StartDate.Should().Be(new DateTime(2022, 2, 3));
        }

        [Fact]
        public void ExceptEndingOn() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptEndingOn(new DateTime(2022, 2, 5));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.EndDate.Should().Be(new DateTime(2022, 2, 5));
        }

        [Fact]
        public void ExceptBetween() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.StartDate.Should().Be(new DateTime(2022, 2, 3));
            result.EndDate.Should().Be(new DateTime(2022, 2, 5));
        }

        [Fact]
        public void Build() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Build();

            result.Should().NotBeNull();
        }
    }
}
