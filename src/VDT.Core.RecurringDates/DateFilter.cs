using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace VDT.Core.RecurringDates;

/// <summary>
/// Filter to invalidate specific dates that would otherwise be valid
/// </summary>
public class DateFilter : IFilter {
    /// <summary>
    /// Gets the dates to invalidate
    /// </summary>
    public ImmutableHashSet<DateTime> Dates { get; }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Create a filter to invalidate specific dates
    /// </summary>
    /// <param name="dates">Dates to invalidate</param>
    public DateFilter(IEnumerable<DateOnly> dates) : this(dates.Select(date => date.ToDateTime())) { }
#endif

    /// <summary>
    /// Create a filter to invalidate specific dates
    /// </summary>
    /// <param name="dates">Dates to invalidate</param>
    public DateFilter(IEnumerable<DateTime> dates) {
        Dates = ImmutableHashSet.CreateRange(dates.Select(date => date.Date));
    }

    /// <inheritdoc/>
    public bool IsFiltered(DateTime date) => Dates.Contains(date);
}
