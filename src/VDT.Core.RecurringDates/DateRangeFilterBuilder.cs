using System;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder for date range filters
    /// </summary>
    public class DateRangeFilterBuilder : IFilterBuilder {
        /// <summary>
        /// Gets or sets the inclusive date from which dates will be invalidated; defaults to <see cref="DateTime.MinValue"/>
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the inclusive date up to which dates will be invalidated; defaults to <see cref="DateTime.MaxValue"/>
        /// </summary>
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// Sets the inclusive start date for this filter
        /// </summary>
        /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
        /// <returns>A reference to this filter builder</returns>
        public DateRangeFilterBuilder From(DateTime? startDate) {
            StartDate = startDate;
            return this;
        }

        /// <summary>
        /// Sets the inclusive end date for this filter
        /// </summary>
        /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
        /// <returns>A reference to this filter builder</returns>
        public DateRangeFilterBuilder Until(DateTime? endDate) {
            EndDate = endDate;
            return this;
        }

        /// <inheritdoc/>
        public IFilter BuildFilter() => new DateRangeFilter(StartDate, EndDate);
    }
}
