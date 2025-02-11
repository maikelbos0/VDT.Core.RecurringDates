#if NET8_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;

namespace VDT.Core.RecurringDates;

/// <summary>
/// <see cref="DateOnly"/> extensions for <see cref="IRecurrenceBuilder"/>
/// </summary>
public static class IRecurrenceBuilderDateOnlyExtensions {
    /// <summary>
    /// Sets the inclusive start date for this recurrence
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
    /// <returns>A reference to this recurrence builder</returns>
    public static IRecurrenceBuilder From(this IRecurrenceBuilder recurrenceBuilder, DateOnly? startDate)
        => recurrenceBuilder.From(startDate?.ToDateTime());

    /// <summary>
    /// Sets the inclusive end date for this recurrence
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
    /// <returns>A reference to this recurrence builder</returns>
    public static IRecurrenceBuilder Until(this IRecurrenceBuilder recurrenceBuilder, DateOnly? endDate)
        => recurrenceBuilder.Until(endDate?.ToDateTime());

    /// <summary>
    /// Adds a filter to this recurrence for the specified dates
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="dates">Dates to invalidate</param>
    /// <returns>A builder to configure the date filter</returns>
    public static DateFilterBuilder ExceptOn(this IRecurrenceBuilder recurrenceBuilder, params DateOnly[] dates)
        => recurrenceBuilder.ExceptOn(dates.Select(date => date.ToDateTime()));

    /// <summary>
    /// Adds a filter to this recurrence for the specified dates
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="dates">Dates to invalidate</param>
    /// <returns>A builder to configure the date filter</returns>
    public static DateFilterBuilder ExceptOn(this IRecurrenceBuilder recurrenceBuilder, IEnumerable<DateOnly> dates)
        => recurrenceBuilder.ExceptOn(dates.Select(date => date.ToDateTime()));

    /// <summary>
    /// Adds a filter to this recurrence for the specified date range
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
    /// <returns>A builder to configure the date range filter</returns>
    public static DateRangeFilterBuilder ExceptFrom(this IRecurrenceBuilder recurrenceBuilder, DateOnly? startDate)
        => recurrenceBuilder.ExceptFrom(startDate?.ToDateTime());

    /// <summary>
    /// Adds a filter to this recurrence for the specified date range
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
    /// <returns>A builder to configure the date range filter</returns>
    public static DateRangeFilterBuilder ExceptUntil(this IRecurrenceBuilder recurrenceBuilder, DateOnly? endDate)
       => recurrenceBuilder.ExceptUntil(endDate?.ToDateTime());

    /// <summary>
    /// Adds a filter to this recurrence for the specified date range
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for the recurrence</param>
    /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
    /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
    /// <returns>A builder to configure the date range filter</returns>
    public static DateRangeFilterBuilder ExceptBetween(this IRecurrenceBuilder recurrenceBuilder, DateOnly? startDate, DateOnly? endDate)
        => recurrenceBuilder.ExceptBetween(startDate?.ToDateTime(), endDate?.ToDateTime());
}
#endif
