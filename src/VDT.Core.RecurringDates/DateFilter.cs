using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Filter to invalidate specific dates that would otherwise be valid
    /// </summary>
    public class DateFilter : IFilter {
        private readonly HashSet<DateTime> dates = new();

        /// <summary>
        /// Dates to invalidate
        /// </summary>
        public IReadOnlyCollection<DateTime> Dates => new ReadOnlyCollection<DateTime>(dates.ToList());

        /// <summary>
        /// Create a filter to invalidate specific dates
        /// </summary>
        /// <param name="dates">Dates to invalidate</param>
        public DateFilter(IEnumerable<DateTime> dates) {
            this.dates.UnionWith(dates.Select(date => date.Date));
        }

        /// <inheritdoc/>
        public bool IsFiltered(DateTime date) => dates.Contains(date);
    }
}
