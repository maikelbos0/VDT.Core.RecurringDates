using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class MonthlyRecurrencePatternBuilderTests {
        [Fact]
        public void On_DaysOfMonth() {
            var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
                DaysOfMonth = new() { 5, 9, 17 }
            };

            builder.Should().BeSameAs(builder.On(11, 19));

            builder.DaysOfMonth.Should().BeEquivalentTo(new[] { 5, 9, 17, 11, 19 });
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(9, 19, -1)]
        [InlineData(int.MinValue)]
        public void On_DaysOfMonth_Throws_For_Invalid_Days(params int[] days) {
            var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Invoking(builder => builder.On(days)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void On_DayOfWeek() {
            var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
                DaysOfWeek = new() {
                    (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                    (DayOfWeekInMonth.Third, DayOfWeek.Friday),
                    (DayOfWeekInMonth.First, DayOfWeek.Monday)
                }
            };

            builder.Should().BeSameAs(builder.On(DayOfWeekInMonth.Third, DayOfWeek.Thursday));

            builder.DaysOfWeek.Should().BeEquivalentTo(new[] {
                (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Third, DayOfWeek.Friday),
                (DayOfWeekInMonth.First, DayOfWeek.Monday),
                (DayOfWeekInMonth.Third, DayOfWeek.Thursday)
            });
        }

        [Fact]
        public void On_DaysOfWeek() {
            var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
                DaysOfWeek = new() {
                    (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                    (DayOfWeekInMonth.Third, DayOfWeek.Friday),
                    (DayOfWeekInMonth.First, DayOfWeek.Monday)
                }
            };

            builder.Should().BeSameAs(builder.On((DayOfWeekInMonth.Second, DayOfWeek.Friday), (DayOfWeekInMonth.Third, DayOfWeek.Thursday)));

            builder.DaysOfWeek.Should().BeEquivalentTo(new[] {
                (DayOfWeekInMonth.First, DayOfWeek.Tuesday),
                (DayOfWeekInMonth.Third, DayOfWeek.Friday),
                (DayOfWeekInMonth.First, DayOfWeek.Monday),
                (DayOfWeekInMonth.Second, DayOfWeek.Friday),
                (DayOfWeekInMonth.Third, DayOfWeek.Thursday)
            });
        }

        [Fact]
        public void On_LastDaysOfMonth() {
            var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1) {
                LastDaysOfMonth = new() { 
                    LastDayOfMonth.Last, 
                    LastDayOfMonth.FourthLast 
                }
            };

            builder.Should().BeSameAs(builder.On(LastDayOfMonth.SecondLast, LastDayOfMonth.FifthLast));

            builder.LastDaysOfMonth.Should().BeEquivalentTo(new[] { 
                LastDayOfMonth.Last, 
                LastDayOfMonth.FourthLast, 
                LastDayOfMonth.SecondLast, 
                LastDayOfMonth.FifthLast 
            });
        }

        [Fact]
        public void WithDaysOfMonthCaching() {
            var builder = new MonthlyRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

            builder.Should().BeSameAs(builder.WithDaysOfMonthCaching());

            builder.CacheDaysOfMonth.Should().BeTrue();
        }

        [Fact]
        public void BuildPattern() {
            var recurrenceBuilder = new RecurrenceBuilder();
            var builder = new MonthlyRecurrencePatternBuilder(recurrenceBuilder, 2) {
                ReferenceDate = new DateTime(2022, 2, 1),
                DaysOfMonth = new() { 3, 5, 20 },
                DaysOfWeek = new() { (DayOfWeekInMonth.Third, DayOfWeek.Thursday), (DayOfWeekInMonth.First, DayOfWeek.Sunday) },
                LastDaysOfMonth = new() { LastDayOfMonth.Last, LastDayOfMonth.FourthLast },
                CacheDaysOfMonth = true
            };

            var result = builder.BuildPattern().Should().BeOfType<MonthlyRecurrencePattern>().Subject;

            result.ReferenceDate.Should().Be(builder.ReferenceDate);
            result.Interval.Should().Be(builder.Interval);
            result.DaysOfMonth.Should().BeEquivalentTo(builder.DaysOfMonth);
            result.DaysOfWeek.Should().BeEquivalentTo(builder.DaysOfWeek);
            result.LastDaysOfMonth.Should().BeEquivalentTo(builder.LastDaysOfMonth);
            result.CacheDaysOfMonth.Should().Be(builder.CacheDaysOfMonth);
        }

        [Fact]
        public void BuildPattern_Takes_StartDate_As_Default_ReferenceDate() {
            var recurrenceBuilder = new RecurrenceBuilder() { StartDate = new DateTime(2022, 2, 1) };
            var builder = new MonthlyRecurrencePatternBuilder(recurrenceBuilder, 2);

            var result = builder.BuildPattern();

            result.ReferenceDate.Should().Be(recurrenceBuilder.StartDate);
        }
    }
}
