﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Pattern for dates that recur every week or every several weeks
    /// </summary>
    public class WeeklyRecurrencePattern : RecurrencePattern {
        private readonly DateTime firstDayOfReferenceWeek;
        private readonly HashSet<DayOfWeek> daysOfWeek = new();

        /// <summary>
        /// Gets the first day of the week to use when calculating the reference week when the interval is greater than 1
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; }

        /// <summary>
        /// Gets the days of the week which are valid for this recurrence pattern
        /// </summary>
        public IReadOnlyList<DayOfWeek> DaysOfWeek => new ReadOnlyCollection<DayOfWeek>(daysOfWeek.ToList());

        /// <summary>
        /// Create a pattern for dates that recur every week or every several weeks
        /// </summary>
        /// <param name="interval">Interval in weeks between occurrences of the pattern to be created</param>
        /// <param name="referenceDate">Date to use as a reference when calculating the reference week when the interval is greater than 1</param>
        /// <param name="firstDayOfWeek">First day of the week to use when calculating the reference week when the interval is greater than 1; defaults to
        /// <see cref="DateTimeFormatInfo.FirstDayOfWeek"/> from <see cref="Thread.CurrentCulture"/></param>
        /// <param name="daysOfWeek">Days of the week which are valid for this recurrence pattern</param>
        public WeeklyRecurrencePattern(int interval, DateTime referenceDate, DayOfWeek? firstDayOfWeek = null, IEnumerable<DayOfWeek>? daysOfWeek = null) : base(interval, referenceDate) {
            FirstDayOfWeek = firstDayOfWeek ?? Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            firstDayOfReferenceWeek = GetFirstDayOfWeekDate(ReferenceDate).Date;

            if (daysOfWeek != null && daysOfWeek.Any()) {
                this.daysOfWeek.UnionWith(daysOfWeek);
            }
            else {
                this.daysOfWeek.Add(referenceDate.DayOfWeek);
            }
        }

        /// <inheritdoc/>
        public override bool IsValid(DateTime date) => daysOfWeek.Contains(date.DayOfWeek) && FitsInterval(date);

        private bool FitsInterval(DateTime date) => Interval == 1 || (GetFirstDayOfWeekDate(date).Date - firstDayOfReferenceWeek).Days % (7 * Interval) == 0;

        private DateTime GetFirstDayOfWeekDate(DateTime date) => date.AddDays((FirstDayOfWeek - date.DayOfWeek - 7) % 7);
    }
}