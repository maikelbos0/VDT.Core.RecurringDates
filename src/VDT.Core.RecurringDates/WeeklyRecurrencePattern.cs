using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Pattern for dates that recur every week or every several weeks
    /// </summary>
    public class WeeklyRecurrencePattern : RecurrencePattern {
        private readonly long reference;

        /// <summary>
        /// Gets the first day of the week to use when calculating the reference week when the interval is greater than 1
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; }

        /// <summary>
        /// Gets the days of the week which are valid for this recurrence pattern
        /// </summary>
        public ImmutableHashSet<DayOfWeek> DaysOfWeek { get; }

        /// <summary>
        /// Create a pattern for dates that recur every week or every several weeks
        /// </summary>
        /// <param name="interval">Interval in weeks between occurrences of the pattern to be created</param>
        /// <param name="referenceDate">Date to use as a reference when calculating the reference week when the interval is greater than 1; defaults to <see cref="DateTime.MinValue"/></param>
        /// <param name="firstDayOfWeek">First day of the week to use when calculating the reference week when the interval is greater than 1; defaults to <see cref="DateTimeFormatInfo.FirstDayOfWeek"/> from <see cref="Thread.CurrentCulture"/></param>
        /// <param name="daysOfWeek">Days of the week which are valid for this recurrence pattern</param>
        /// <remarks>
        /// If <paramref name="daysOfWeek"/> is empty, the day of the week of <paramref name="referenceDate"/> will be added to <see cref="DaysOfWeek"/>.
        /// </remarks>
        public WeeklyRecurrencePattern(int interval, DateTime? referenceDate, DayOfWeek? firstDayOfWeek = null, IEnumerable<DayOfWeek>? daysOfWeek = null) : base(interval, referenceDate) {
            FirstDayOfWeek = firstDayOfWeek ?? Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            reference = GetIntervalReference(ReferenceDate);

            if (daysOfWeek != null && daysOfWeek.Any()) {
                DaysOfWeek = ImmutableHashSet.CreateRange(daysOfWeek);
            }
            else {
                DaysOfWeek = ImmutableHashSet.Create(ReferenceDate.DayOfWeek);
            }
        }

        /// <inheritdoc/>
        public override bool IsValid(DateTime date) => DaysOfWeek.Contains(date.DayOfWeek) && FitsInterval(date);

        private bool FitsInterval(DateTime date) => Interval == 1 || (GetIntervalReference(date) - reference) % (7 * Interval) == 0;

        private long GetIntervalReference(DateTime date) => date.Ticks / TimeSpan.TicksPerDay + (FirstDayOfWeek - date.DayOfWeek - 7) % 7;
    }
}