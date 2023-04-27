using System;
using System.Collections.Generic;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder for date recurrences
    /// </summary>
    public interface IRecurrenceBuilder {
        /// <summary>
        /// Sets the inclusive start date for this recurrence
        /// </summary>
        /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
        /// <returns>A reference to this recurrence builder</returns>
        IRecurrenceBuilder From(DateTime? startDate);

        /// <summary>
        /// Sets the inclusive end date for this recurrence
        /// </summary>
        /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
        /// <returns>A reference to this recurrence builder</returns>
        IRecurrenceBuilder Until(DateTime? endDate);

        /// <summary>
        /// Sets the maximum number of occurrences for this recurrence
        /// </summary>
        /// <param name="occurrences">The maximum number of occurrences; if <see langword="null"/> it repeats without limit</param>
        /// <returns>A reference to this recurrence builder</returns>
        IRecurrenceBuilder StopAfter(int? occurrences);

        /// <summary>
        /// Adds a pattern to this recurrence to repeat daily
        /// </summary>
        /// <returns>A builder to configure the new daily pattern</returns>
        DailyRecurrencePatternBuilder Daily();

        /// <summary>
        /// Adds a pattern to this recurrence to repeat weekly
        /// </summary>
        /// <returns>A builder to configure the new weekly pattern</returns>
        WeeklyRecurrencePatternBuilder Weekly();

        /// <summary>
        /// Adds a pattern to this recurrence to repeat monthly
        /// </summary>
        /// <returns>A builder to configure the new monthly pattern</returns>
        MonthlyRecurrencePatternBuilder Monthly();

        /// <summary>
        /// Allows for adding recurrence patterns that repeat with the provided interval
        /// </summary>
        /// <param name="interval">The interval with which to repeat the new recurrence pattern</param>
        /// <returns>A starting point to add recurrence patterns that repeat with the provided interval</returns>
        RecurrencePatternBuilderStart Every(int interval);

        /// <summary>
        /// Enable caching of date validity; if you use custom patterns that can be edited the cache should not be enabled
        /// </summary>
        /// <returns>A reference to this recurrence pattern builder</returns>
        RecurrenceBuilder WithDateCaching();

        /// <summary>
        /// Adds a filter to this recurrence for the specified dates
        /// </summary>
        /// <param name="dates">Dates to invalidate</param>
        /// <returns>A builder to configure the date filter</returns>
        DateFilterBuilder ExceptOn(params DateTime[] dates);

        /// <summary>
        /// Adds a filter to this recurrence for the specified dates
        /// </summary>
        /// <param name="dates">Dates to invalidate</param>
        /// <returns>A builder to configure the date filter</returns>
        DateFilterBuilder ExceptOn(IEnumerable<DateTime> dates);

        /// <summary>
        /// Adds a filter to this recurrence for the specified date range
        /// </summary>
        /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
        /// <returns>A builder to configure the date range filter</returns>
        DateRangeFilterBuilder ExceptFrom(DateTime? startDate);

        /// <summary>
        /// Adds a filter to this recurrence for the specified date range
        /// </summary>
        /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
        /// <returns>A builder to configure the date range filter</returns>
        DateRangeFilterBuilder ExceptUntil(DateTime? endDate);

        /// <summary>
        /// Adds a filter to this recurrence for the specified date range
        /// </summary>
        /// <param name="startDate">The inclusive start date; defaults to <see cref="DateTime.MinValue"/></param>
        /// <param name="endDate">The inclusive end date; defaults to <see cref="DateTime.MaxValue"/></param>
        /// <returns>A builder to configure the date range filter</returns>
        DateRangeFilterBuilder ExceptBetween(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Adds a filter to this recurrence for the specified recurrence
        /// </summary>
        /// <param name="recurrence">Recurrence to check for dates to invalidate</param>
        /// <returns>A builder to configure the recurrence filter</returns>
        RecurrenceFilterBuilder ExceptIntersecting(Recurrence recurrence);

        /// <summary>
        /// Build the recurrence based on the provided specifications and patterns
        /// </summary>
        /// <returns>The composed recurrence</returns>
        Recurrence Build();
    }
}