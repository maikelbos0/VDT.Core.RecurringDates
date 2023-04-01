using System;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Filter to determine when an otherwise valid date should be invalidated in a recurrence
    /// </summary>
    public interface IFilter {
        /// <summary>
        /// Determine whether a given date should be invalidated
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns><see langword="true"/> if the given date should be filtered out according to this filter; otherwise <see langword="false"/></returns>
        bool IsFiltered(DateTime date);
    }
}
