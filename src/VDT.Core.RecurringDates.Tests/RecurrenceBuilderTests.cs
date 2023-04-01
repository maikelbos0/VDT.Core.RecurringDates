using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrenceBuilderTests {
        [Fact]
        public void From() {
            var builder = new RecurrenceBuilder();

            builder.Should().BeSameAs(builder.From(new DateTime(2022, 1, 1)));

            builder.StartDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void Until() {
            var builder = new RecurrenceBuilder();

            builder.Should().BeSameAs(builder.Until(new DateTime(2022, 12, 31)));

            builder.EndDate.Should().Be(new DateTime(2022, 12, 31));
        }

        [Fact]
        public void StopAfter() {
            var builder = new RecurrenceBuilder();

            builder.Should().BeSameAs(builder.StopAfter(10));

            builder.Occurrences.Should().Be(10);
        }

        [Fact]
        public void Daily() {
            var builder = new RecurrenceBuilder();

            var result = builder.Daily();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Weekly() {
            var builder = new RecurrenceBuilder();

            var result = builder.Weekly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Monthly() {
            var builder = new RecurrenceBuilder();

            var result = builder.Monthly();

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.PatternBuilders.Should().Contain(result);
            result.Interval.Should().Be(1);
        }

        [Fact]
        public void Every() {
            var builder = new RecurrenceBuilder();

            var result = builder.Every(2);

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            result.Interval.Should().Be(2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Every_Throws_For_Invalid_Interval(int interval) {
            var builder = new RecurrenceBuilder();

            builder.Invoking(builder => builder.Every(interval)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithDateCaching() {
            var builder = new RecurrenceBuilder();

            builder.Should().BeSameAs(builder.WithDateCaching());

            builder.CacheDates.Should().BeTrue();
        }

        [Fact]
        public void ExceptOn() {
            var builder = new RecurrenceBuilder();

            var result = builder.ExceptOn(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.FilterBuilders.Should().Contain(result);
            result.Dates.Should().Equal(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));
        }

        [Fact]
        public void ExceptFrom() {
            var builder = new RecurrenceBuilder();

            var result = builder.ExceptFrom(new DateTime(2022, 2, 3));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.FilterBuilders.Should().Contain(result);
            result.StartDate.Should().Be(new DateTime(2022, 2, 3));
        }

        [Fact]
        public void ExceptUntil() {
            var builder = new RecurrenceBuilder();

            var result = builder.ExceptUntil(new DateTime(2022, 2, 5));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.FilterBuilders.Should().Contain(result);
            result.EndDate.Should().Be(new DateTime(2022, 2, 5));
        }

        [Fact]
        public void ExceptBetween() {
            var builder = new RecurrenceBuilder();

            var result = builder.ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));

            result.RecurrenceBuilder.Should().BeSameAs(builder);
            builder.FilterBuilders.Should().Contain(result);
            result.StartDate.Should().Be(new DateTime(2022, 2, 3));
            result.EndDate.Should().Be(new DateTime(2022, 2, 5));
        }

        [Fact]
        public void Build() {
            var builder = new RecurrenceBuilder() {
                StartDate = new DateTime(2022, 1, 1),
                EndDate = new DateTime(2022, 12, 31),
                Occurrences = 10
            };

            builder.PatternBuilders.Add(new DailyRecurrencePatternBuilder(builder, 3));
            builder.PatternBuilders.Add(new WeeklyRecurrencePatternBuilder(builder, 2));
            builder.FilterBuilders.Add(new DateFilterBuilder(builder));
            builder.FilterBuilders.Add(new DateRangeFilterBuilder(builder));

            var result = builder.Build();

            result.StartDate.Should().Be(builder.StartDate);
            result.EndDate.Should().Be(builder.EndDate);
            result.Occurrences.Should().Be(builder.Occurrences);
            result.Patterns.Should().HaveSameCount(builder.PatternBuilders);
            result.Patterns.Should().ContainSingle(pattern => pattern is DailyRecurrencePattern && pattern.Interval == 3);
            result.Patterns.Should().ContainSingle(pattern => pattern is WeeklyRecurrencePattern && pattern.Interval == 2);
            result.Filters.Should().HaveSameCount(builder.FilterBuilders);
            result.Filters.Should().ContainSingle(filter => filter is DateFilter);
            result.Filters.Should().ContainSingle(filter => filter is DateRangeFilter);
        }
    }
}
