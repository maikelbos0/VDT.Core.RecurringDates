using System;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Filter to invalidate date ranges that would otherwise be valid
    /// </summary>
    public class DateRangeFilter : IFilter {
        /// <summary>
        /// Gets the inclusive date from which dates will be invalidated
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// Gets the inclusive date up to which dates will be invalidated
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// Create a filter to invalidate date ranges
        /// </summary>
        /// <param name="startDate">Inclusive date from which dates will be invalidated; defaults to <see cref="DateTime.MinValue"/></param>
        /// <param name="endDate">Inclusive date up to which dates will be invalidated; defaults to <see cref="DateTime.MaxValue"/></param>
        public DateRangeFilter(DateTime? startDate, DateTime? endDate) {
            StartDate = startDate?.Date ?? DateTime.MinValue;
            EndDate = endDate?.Date ?? DateTime.MaxValue;
        }

        /// <inheritdoc/>
        public bool IsFiltered(DateTime date) => date >= StartDate && date <= EndDate;
    }
}
