using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DailyRecurrencePatternBuilderTests {
        [Fact]
        public void WithWeekendHandling() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            Assert.Same(builder, builder.WithWeekendHandling(RecurrencePatternWeekendHandling.AdjustToFriday));

            Assert.Equal(builder.WeekendHandling, RecurrencePatternWeekendHandling.AdjustToFriday);
        }

        [Fact]
        public void IncludeWeekends() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            Assert.Same(builder, builder.IncludeWeekends());

            Assert.Equal(builder.WeekendHandling, RecurrencePatternWeekendHandling.Include);
        }

        [Fact]
        public void SkipWeekends() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            Assert.Same(builder, builder.SkipWeekends());

            Assert.Equal(builder.WeekendHandling, RecurrencePatternWeekendHandling.Skip);
        }

        [Fact]
        public void AdjustWeekendsToMonday() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            Assert.Same(builder, builder.AdjustWeekendsToMonday());

            Assert.Equal(builder.WeekendHandling, RecurrencePatternWeekendHandling.AdjustToMonday);
        }

        [Fact]
        public void AdjustWeekendsToFriday() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            Assert.Same(builder, builder.AdjustWeekendsToFriday());

            Assert.Equal(builder.WeekendHandling, RecurrencePatternWeekendHandling.AdjustToFriday);
        }

        [Fact]
        public void AdjustWeekendsToWeekday() {
            var builder = new DailyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            Assert.Same(builder, builder.AdjustWeekendsToWeekday());

            Assert.Equal(builder.WeekendHandling, RecurrencePatternWeekendHandling.AdjustToWeekday);
        }

        [Fact]
        public void BuildPattern() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var builder = new DailyRecurrencePatternBuilder(recurrenceBuilder, 2) {
                ReferenceDate = new DateTime(2022, 2, 1),
                WeekendHandling = RecurrencePatternWeekendHandling.Skip
            };

            var result = Assert.IsType<DailyRecurrencePattern>(builder.BuildPattern());

            Assert.Equal(builder.ReferenceDate, result.ReferenceDate);
            Assert.Equal(builder.Interval, result.Interval);
            Assert.Equal(builder.WeekendHandling, result.WeekendHandling);
        }

        [Fact]
        public void BuildPattern_Takes_StartDate_As_Default_ReferenceDate() {
            var recurrenceBuilder = new RecurrenceBuilder() { StartDate = new DateTime(2022, 2, 1) };
            var builder = new DailyRecurrencePatternBuilder(recurrenceBuilder, 2);

            Assert.Equal(recurrenceBuilder.StartDate, builder.BuildPattern().ReferenceDate);
        }
    }
}
