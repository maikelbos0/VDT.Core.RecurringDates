using System;
using System.Collections.Generic;
using System.Linq;

namespace VDT.Core.RecurringDates;

/// <summary>
/// Builder for date filters
/// </summary>
public class DateFilterBuilder : FilterBuilder {
    /// <summary>
    /// Gets or sets the dates to invalidate
    /// </summary>
    public List<DateTime> Dates { get; set; } = new List<DateTime>();

    /// <summary>
    /// Create a builder for composing a filter to invalidate specific dates
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for date recurrences to which this filter builder belongs</param>
    public DateFilterBuilder(RecurrenceBuilder recurrenceBuilder) : base(recurrenceBuilder) { }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Adds the given dates to the dates this filter invalidates
    /// </summary>
    /// <param name="dates">Dates that should be added</param>
    /// <returns>A reference to this filter builder</returns>
    public DateFilterBuilder On(params DateOnly[] dates) => On(dates.Select(date => date.ToDateTime()));

    /// <summary>
    /// Adds the given dates to the dates this filter invalidates
    /// </summary>
    /// <param name="dates">Dates that should be added</param>
    /// <returns>A reference to this filter builder</returns>
    public DateFilterBuilder On(IEnumerable<DateOnly> dates) => On(dates.Select(date => date.ToDateTime()));
#endif

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
    public override IFilter BuildFilter() => new DateFilter(Dates);
}
