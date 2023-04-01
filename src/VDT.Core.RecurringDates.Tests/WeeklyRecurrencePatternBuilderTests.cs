using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class WeeklyRecurrencePatternBuilderTests {
        [Fact]
        public void UsingFirstDayOfWeek() {
            var builder = new WeeklyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.UsingFirstDayOfWeek(DayOfWeek.Tuesday).Should().BeSameAs(builder);

            builder.FirstDayOfWeek.Should().Be(DayOfWeek.Tuesday);
        }

        [Fact]
        public void On() {
            var builder = new WeeklyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
                DaysOfWeek = new() {
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday
                }
            };

            builder.On(DayOfWeek.Friday, DayOfWeek.Monday).Should().BeSameAs(builder);

            builder.DaysOfWeek.Should().Equal(
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Monday
            );
        }

        [Fact]
        public void BuildPattern() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var builder = new WeeklyRecurrencePatternBuilder(recurrenceBuilder, 2) {
                ReferenceDate = new DateTime(2022, 2, 1),
                FirstDayOfWeek = DayOfWeek.Wednesday,
                DaysOfWeek = new() { DayOfWeek.Sunday, DayOfWeek.Thursday }
            };

            var result = builder.BuildPattern().Should().BeOfType<WeeklyRecurrencePattern>().Subject;

            result.ReferenceDate.Should().Be(builder.ReferenceDate);
            result.Interval.Should().Be(builder.Interval);
            result.FirstDayOfWeek.Should().Be(builder.FirstDayOfWeek);
            result.DaysOfWeek.Should().BeEquivalentTo(builder.DaysOfWeek);
        }

        [Fact]
        public void BuildPattern_Takes_StartDate_As_Default_ReferenceDate() {
            var recurrenceBuilder = new RecurrenceBuilder() { StartDate = new DateTime(2022, 2, 1) };
            var builder = new WeeklyRecurrencePatternBuilder(recurrenceBuilder, 2);

            var result = builder.BuildPattern();

            result.ReferenceDate.Should().Be(recurrenceBuilder.StartDate);
        }
    }
}
