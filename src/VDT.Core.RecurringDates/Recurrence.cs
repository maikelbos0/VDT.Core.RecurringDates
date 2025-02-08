using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace VDT.Core.RecurringDates;

/// <summary>
/// A recurrence to determine valid dates for the given patterns
/// </summary>
public class Recurrence {
    private readonly ConcurrentDictionary<DateTime, bool>? dateCache;

    /// <summary>
    /// Gets the inclusive start date for this recurrence
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Gets the inclusive end date for this recurrence
    /// </summary>
    public DateTime EndDate { get; }

    /// <summary>
    /// Gets the maximum number of occurrences for this recurrence; if <see langword="null"/> it repeats without limit
    /// </summary>
    public int? Occurrences { get; }

    /// <summary>
    /// Gets the recurrence patterns that this recurrence will use to determine valid dates
    /// </summary>
    public ImmutableList<RecurrencePattern> Patterns { get; }

    /// <summary>
    /// Filters that this recurrence will use to invalidate otherwise valid dates
    /// </summary>
    public ImmutableList<IFilter> Filters { get; }

    /// <summary>
    /// Indicates whether or not date validity should be cached; if you use custom patterns that can be edited the cache may need to be disabled
    /// </summary>
    public bool CacheDates => dateCache != null;

#if NET8_0_OR_GREATER
    /// <summary>
    /// Create a recurrence to determine valid dates for the given patterns
    /// </summary>
    /// <param name="startDate">Inclusive start date for this recurrence; defaults to <see cref="DateTime.MinValue"/></param>
    /// <param name="endDate">Inclusive end date for this recurrence; defaults to <see cref="DateTime.MaxValue"/></param>
    /// <param name="occurrences">Maximum number of occurrences for this recurrence</param>
    /// <param name="patterns">Recurrence patterns that this recurrence will use to determine valid dates</param>
    /// <param name="filters">Filters that this recurrence will use to invalidate otherwise valid dates</param>
    /// <param name="cacheDates">Indicates whether or not date validity should be cached; if you use custom patterns that can be edited the cache may need to be disabled</param>
    public Recurrence(DateOnly? startDate, DateOnly? endDate, int? occurrences, IEnumerable<RecurrencePattern> patterns, IEnumerable<IFilter> filters, bool cacheDates)
        : this(startDate?.ToDateTime(), endDate?.ToDateTime(), occurrences, patterns, filters, cacheDates) { }
#endif

    /// <summary>
    /// Create a recurrence to determine valid dates for the given patterns
    /// </summary>
    /// <param name="startDate">Inclusive start date for this recurrence; defaults to <see cref="DateTime.MinValue"/></param>
    /// <param name="endDate">Inclusive end date for this recurrence; defaults to <see cref="DateTime.MaxValue"/></param>
    /// <param name="occurrences">Maximum number of occurrences for this recurrence</param>
    /// <param name="patterns">Recurrence patterns that this recurrence will use to determine valid dates</param>
    /// <param name="filters">Filters that this recurrence will use to invalidate otherwise valid dates</param>
    /// <param name="cacheDates">Indicates whether or not date validity should be cached; if you use custom patterns that can be edited the cache may need to be disabled</param>
    public Recurrence(DateTime? startDate, DateTime? endDate, int? occurrences, IEnumerable<RecurrencePattern> patterns, IEnumerable<IFilter> filters, bool cacheDates) {
        StartDate = (startDate ?? DateTime.MinValue).Date;
        EndDate = (endDate ?? DateTime.MaxValue).Date;
        Occurrences = occurrences;
        Patterns = ImmutableList.CreateRange(patterns);
        Filters = ImmutableList.CreateRange(filters);

        if (cacheDates) {
            dateCache = new();
        }
    }

    /// <summary>
    /// Gets valid dates for this recurrence within the specified optional range
    /// </summary>
    /// <param name="from">If provided, no dates before this date will be returned</param>
    /// <param name="until">If provided, no dates after this date will be returned</param>
    /// <returns>Valid dates for this recurrence within the specified optional range</returns>
    public IEnumerable<DateTime> GetDates(DateTime? from = null, DateTime? until = null) {
        if (from == null || from < StartDate) {
            from = StartDate;
        }
        else {
            from = from.Value.Date;
        }

        if (until == null || until > EndDate) {
            until = EndDate;
        }

        if (Occurrences == null) {
            var currentDate = from.Value;

            while (true) {
                if (IsValidInPatternsAndFilters(currentDate)) {
                    yield return currentDate;
                }

                if (currentDate >= until) {
                    break;
                }

                currentDate = currentDate.AddDays(1);
            }
        }
        else {
            var occurrences = 0;
            var currentDate = StartDate;

            while (true) {
                if (IsValidInPatternsAndFilters(currentDate)) {
                    occurrences++;

                    if (currentDate >= from) {
                        yield return currentDate;
                    }
                }

                if (currentDate >= until || occurrences >= Occurrences) {
                    break;
                }

                currentDate = currentDate.AddDays(1);
            }
        }
    }

    /// <summary>
    /// Determine whether a given date is valid according to this recurrence
    /// </summary>
    /// <param name="date">Date to check</param>
    /// <returns><see langword="true"/> if the given date is valid according to this recurrence; otherwise <see langword="false"/></returns>
    public bool IsValid(DateTime date) {
        date = date.Date;

        if (date < StartDate || date > EndDate) {
            return false;
        }

        if (Occurrences == null) {
            return IsValidInPatternsAndFilters(date);
        }
        else {
            var occurrences = 0;
            var currentDate = StartDate;

            while (currentDate < date && Occurrences > occurrences) {
                if (IsValidInPatternsAndFilters(currentDate)) {
                    occurrences++;
                }

                currentDate = currentDate.AddDays(1);
            }

            return occurrences < Occurrences && IsValidInPatternsAndFilters(date);
        }
    }

    internal bool IsValidInPatternsAndFilters(DateTime date) {
        return dateCache?.GetOrAdd(date, IsValidInPatternsAndFiltersInternal) ?? IsValidInPatternsAndFiltersInternal(date);

        bool IsValidInPatternsAndFiltersInternal(DateTime date) => Patterns.Any(pattern => pattern.IsValid(date)) && !Filters.Any(filter => filter.IsFiltered(date));
    }
}