using System;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Filter to invalidate dates from another recurrence
    /// </summary>
    public class RecurrenceFilter : IFilter {
        /// <summary>
        /// Gets the recurrence to check for dates to invalidate
        /// </summary>
        public Recurrence Recurrence { get; }

        /// <summary>
        /// Create a filter to invalidate dates from another recurrence
        /// </summary>
        /// <param name="recurrence">Recurrence to check for dates to invalidate</param>
        public RecurrenceFilter(Recurrence recurrence)
        {
            Recurrence = recurrence;
        }

        /// <inheritdoc/>
        public bool IsFiltered(DateTime date) => Recurrence.IsValid(date);
    }
}
