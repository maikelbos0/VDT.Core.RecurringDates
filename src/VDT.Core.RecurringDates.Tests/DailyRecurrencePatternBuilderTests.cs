using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DailyRecurrencePatternBuilderTests {
        [Fact]
        public void WithWeekendHandling() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.WithWeekendHandling(RecurrencePatternWeekendHandling.AdjustToFriday));

            builder.WeekendHandling.Should().Be(RecurrencePatternWeekendHandling.AdjustToFriday);
        }

        [Fact]
        public void IncludeWeekends() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.IncludeWeekends());

            builder.WeekendHandling.Should().Be(RecurrencePatternWeekendHandling.Include);
        }

        [Fact]
        public void SkipWeekends() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.SkipWeekends());

            builder.WeekendHandling.Should().Be(RecurrencePatternWeekendHandling.Skip);
        }

        [Fact]
        public void AdjustWeekendsToMonday() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.AdjustWeekendsToMonday());

            builder.WeekendHandling.Should().Be(RecurrencePatternWeekendHandling.AdjustToMonday);
        }

        [Fact]
        public void AdjustWeekendsToFriday() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.AdjustWeekendsToFriday());

            builder.WeekendHandling.Should().Be(RecurrencePatternWeekendHandling.AdjustToFriday);
        }

        [Fact]
        public void AdjustWeekendsToWeekday() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.AdjustWeekendsToWeekday());

            builder.WeekendHandling.Should().Be(RecurrencePatternWeekendHandling.AdjustToWeekday);
        }

        [Fact]
        public void BuildPattern() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var builder = new DailyRecurrencePatternBuilder(recurrenceBuilder, 2) {
                ReferenceDate = new DateTime(2022, 2, 1),
                WeekendHandling = RecurrencePatternWeekendHandling.Skip
            };

            var result = builder.BuildPattern().Should().BeOfType<DailyRecurrencePattern>().Subject;

            result.ReferenceDate.Should().Be(builder.ReferenceDate);
            result.Interval.Should().Be(builder.Interval);
            result.WeekendHandling.Should().Be(builder.WeekendHandling);
        }

        [Fact]
        public void BuildPattern_Takes_StartDate_As_Default_ReferenceDate() {
            var recurrenceBuilder = new RecurrenceBuilder() { StartDate = new DateTime(2022, 2, 1) };
            var builder = new DailyRecurrencePatternBuilder(recurrenceBuilder, 2);

            builder.BuildPattern().ReferenceDate.Should().Be(recurrenceBuilder.StartDate);
        }
    }
}
