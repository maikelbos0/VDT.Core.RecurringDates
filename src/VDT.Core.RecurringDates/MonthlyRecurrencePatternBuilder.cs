﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder for composing patterns for dates that recur every month or every several months
    /// </summary>
    public class MonthlyRecurrencePatternBuilder : RecurrencePatternBuilder<MonthlyRecurrencePatternBuilder> {
        /// <summary>
        /// Gets or sets the days of the month which are valid for this recurrence pattern
        /// </summary>
        public List<int> DaysOfMonth { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the ordinal days of the week (e.g. the second Thursday of the month) which are valid for this recurrence pattern
        /// </summary>
        public List<(DayOfWeekInMonth, DayOfWeek)> DaysOfWeek { get; set; } = new List<(DayOfWeekInMonth, DayOfWeek)>();

        /// <summary>
        /// Gets or sets the last days of the month which are valid for this recurrence pattern
        /// pattern
        /// </summary>
        public List<LastDayOfMonth> LastDaysOfMonth { get; set; } = new List<LastDayOfMonth>();

        /// <summary>
        /// Indicates whether or not days of specific months should be cached; enable this cache to significantly speed up this pattern type at the cost of
        /// memory; defaults to <see langword="false"/>
        /// </summary>
        public bool CacheDaysOfMonth { get; set; }

        /// <summary>
        /// Create a builder for composing patterns for dates that recur every month or every several months
        /// </summary>
        /// <param name="recurrenceBuilder">Builder for date recurrences to which this pattern builder belongs</param>
        /// <param name="interval">Interval in months between occurrences of the pattern to be created</param>
        public MonthlyRecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) : base(recurrenceBuilder, interval) { }

        /// <summary>
        /// Adds the given days of the month to the valid days of the month for this recurrence pattern
        /// </summary>
        /// <param name="days">Days of the month that should be added</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(params int[] days)
            => On(days.AsEnumerable());

        /// <summary>
        /// Adds the given days of the month to the valid days of the month for this recurrence pattern
        /// </summary>
        /// <param name="days">Days of the month that should be added</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(IEnumerable<int> days) {
            DaysOfMonth.AddRange(Guard.ArePositive(days));
            return this;
        }

        /// <summary>
        /// Adds the given ordinal day of the week (e.g. the second Thursday of the month) to the valid ordinal days of the week for this recurrence pattern
        /// </summary>
        /// <param name="dayOfWeekInMonth">Ordinal</param>
        /// <param name="dayOfWeek">Day of the week</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(DayOfWeekInMonth dayOfWeekInMonth, DayOfWeek dayOfWeek)
            => On((dayOfWeekInMonth, dayOfWeek));

        /// <summary>
        /// Adds the given ordinal days of the week (e.g. the second Thursday of the month) to the valid ordinal days of the week for this recurrence pattern
        /// </summary>
        /// <param name="days">Ordinal days of the week that should be added</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(params (DayOfWeekInMonth, DayOfWeek)[] days)
            => On(days.AsEnumerable());

        /// <summary>
        /// Adds the given ordinal days of the week (e.g. the second Thursday of the month) to the valid ordinal days of the week for this recurrence pattern
        /// </summary>
        /// <param name="days">Ordinal days of the week that should be added</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(IEnumerable<(DayOfWeekInMonth, DayOfWeek)> days) {
            DaysOfWeek.AddRange(days);
            return this;
        }

        /// <summary>
        /// Adds the given last days of the month to the last days of the month which are valid for this recurrence pattern
        /// </summary>
        /// <param name="days">Last days of the month that should be added</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(params LastDayOfMonth[] days)
            => On(days.AsEnumerable());

        /// <summary>
        /// Adds the given last days of the month to the last days of the month which are valid for this recurrence pattern
        /// </summary>
        /// <param name="days">Last days of the month that should be added</param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder On(IEnumerable<LastDayOfMonth> days) {
            LastDaysOfMonth.AddRange(days);
            return this;
        }

        /// <summary>
        /// Enable caching of days of specific months
        /// </summary>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public MonthlyRecurrencePatternBuilder WithDaysOfMonthCaching() {
            CacheDaysOfMonth = true;
            return this;
        }

        /// <inheritdoc/>
        public override RecurrencePattern BuildPattern()
            => new MonthlyRecurrencePattern(Interval, ReferenceDate ?? RecurrenceBuilder.StartDate, DaysOfMonth, DaysOfWeek, LastDaysOfMonth, CacheDaysOfMonth);
    }
}