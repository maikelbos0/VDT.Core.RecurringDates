#if NET8_0_OR_GREATER
using System;

namespace VDT.Core.RecurringDates;

/// <summary>
/// <see cref="DateOnly"/> extensions for <see cref="IFilter"/>
/// </summary>
public static class IFilterDateOnlyExtensions {
    /// <summary>
    /// Determine whether a given date should be invalidated
    /// </summary>
    /// <param name="filter">Filter</param>
    /// <param name="date">Date to check</param>
    /// <returns><see langword="true"/> if the given date should be filtered out according to this filter; otherwise <see langword="false"/></returns>
    public static bool IsFiltered(this IFilter filter, DateOnly date) => filter.IsFiltered(date.ToDateTime());
}
#endif
