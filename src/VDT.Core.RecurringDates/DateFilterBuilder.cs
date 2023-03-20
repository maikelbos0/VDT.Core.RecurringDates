using System;
using System.Collections.Generic;
using System.Linq;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder for date filters
    /// </summary>
    public class DateFilterBuilder : IFilterBuilder {
        /// <summary>
        /// Gets or sets the dates to invalidate
        /// </summary>
        public List<DateTime> Dates { get; set; } = new List<DateTime>();

        /// <summary>
        /// Adds the given dates to the dates this filter invalidates
        /// </summary>
        /// <param name="dates">Dates that should be added</param>
        /// <returns>A reference to this filter builder</returns>
        public DateFilterBuilder On(params DateTime[] dates) => On(dates.AsEnumerable());

        /// <summary>
        /// Adds the given dates to the dates this filter invalidates
        /// </summary>
        /// <param name="dates">Dates that should be added</param>
        /// <returns>A reference to this filter builder</returns>
        public DateFilterBuilder On(IEnumerable<DateTime> dates) {
            Dates.AddRange(dates);
            return this;
        }

        /// <inheritdoc/>
        public IFilter BuildFilter() => new DateFilter(Dates);
    }
}
