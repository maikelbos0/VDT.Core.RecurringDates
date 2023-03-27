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
            var patternBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(patternBuilder.From(new DateTime(2022, 1, 1)));

            builder.StartDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void Until() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(patternBuilder.Until(new DateTime(2022, 12, 31)));

            builder.EndDate.Should().Be(new DateTime(2022, 12, 31));
        }

        [Fact]
        public void StopAfter() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(patternBuilder.StopAfter(10));

            builder.Occurrences.Should().Be(10);
        }

        [Fact]
        public void Daily() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            var result = patternBuilder.Daily();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Weekly() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            var result = patternBuilder.Weekly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Monthly() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            var result = patternBuilder.Monthly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Every() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            var result = patternBuilder.Every(2);

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.Interval.Should().Be(2);
        }

        [Fact]
        public void WithDateCaching() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            builder.Should().BeSameAs(patternBuilder.WithDateCaching());

            builder.CacheDates.Should().BeTrue();
        }

        [Fact]
        public void Build() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestFilterBuilder(builder);

            var result = patternBuilder.Build();

            result.Should().NotBeNull();
        }
    }
}
