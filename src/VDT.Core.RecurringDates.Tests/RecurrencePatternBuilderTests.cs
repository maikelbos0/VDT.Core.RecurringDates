using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrencePatternBuilderTests {
        private class TestRecurrencePatternBuilder : RecurrencePatternBuilder<TestRecurrencePatternBuilder> {
            public TestRecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) : base(recurrenceBuilder, interval) { }

            public override RecurrencePattern BuildPattern() => throw new NotImplementedException();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Constructor_Throws_For_Invalid_Interval(int interval) {
            var action = () => new TestRecurrencePatternBuilder(new RecurrenceBuilder(), interval);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithReferenceDate() {
            var builder = new TestRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.WithReferenceDate(new DateTime(2022, 2, 1)));

            builder.ReferenceDate.Should().Be(new DateTime(2022, 2, 1));
        }

        [Fact]
        public void From() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            builder.Should().BeSameAs(patternBuilder.From(new DateTime(2022, 1, 1)));

            builder.StartDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void Until() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            builder.Should().BeSameAs(patternBuilder.Until(new DateTime(2022, 12, 31)));

            builder.EndDate.Should().Be(new DateTime(2022, 12, 31));
        }

        [Fact]
        public void StopAfter() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            builder.Should().BeSameAs(patternBuilder.StopAfter(10));

            builder.Occurrences.Should().Be(10);
        }

        [Fact]
        public void Daily() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.Daily();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Weekly() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.Weekly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Monthly() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.Monthly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Every() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.Every(2);

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.Interval.Should().Be(2);
        }

        [Fact]
        public void WithDateCaching() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            builder.Should().BeSameAs(patternBuilder.WithDateCaching());

            builder.CacheDates.Should().BeTrue();
        }

        [Fact]
        public void ExceptOn() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.ExceptOn(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.Dates.Should().Equal(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));
        }

        [Fact]
        public void ExceptFrom() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.ExceptFrom(new DateTime(2022, 2, 3));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.StartDate.Should().Be(new DateTime(2022, 2, 3));
        }

        [Fact]
        public void ExceptUntil() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.ExceptUntil(new DateTime(2022, 2, 5));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.EndDate.Should().Be(new DateTime(2022, 2, 5));
        }

        [Fact]
        public void ExceptBetween() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.StartDate.Should().Be(new DateTime(2022, 2, 3));
            result.EndDate.Should().Be(new DateTime(2022, 2, 5));
        }

        [Fact]
        public void Build() {
            var builder = new RecurrenceBuilder();
            var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

            var result = patternBuilder.Build();

            result.Should().NotBeNull();
        }
    }
}
