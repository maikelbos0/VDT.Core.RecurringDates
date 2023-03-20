﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace VDT.Core.RecurringDates {
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
            StartDate = startDate?.Date ?? DateTime.MinValue;
            EndDate = endDate?.Date ?? DateTime.MaxValue;
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
        /// <param name="to">If provided, no dates after this date will be returned</param>
        /// <returns>Valid dates for this recurrence within the specified optional range</returns>
        public IEnumerable<DateTime> GetDates(DateTime? from = null, DateTime? to = null) {
            if (from == null || from < StartDate) {
                from = StartDate;
            }

            if (to == null || to > EndDate) {
                to = EndDate;
            }

            if (Occurrences == null) {
                return GetDatesWithoutOccurrences(from.Value.Date, to.Value.Date);
            }
            else {
                return GetDatesWithOccurrences(from.Value.Date, to.Value.Date);
            }

            IEnumerable<DateTime> GetDatesWithoutOccurrences(DateTime from, DateTime to) {
                var currentDate = from;

                while (currentDate <= to) {
                    if (IsValidInPatternsAndFilters(currentDate)) {
                        yield return currentDate;
                    }

                    currentDate = currentDate.AddDays(1);
                }
            }

            IEnumerable<DateTime> GetDatesWithOccurrences(DateTime from, DateTime to) {
                var occurrences = 0;
                var currentDate = StartDate;

                while (currentDate <= to && Occurrences > occurrences) {
                    if (IsValidInPatternsAndFilters(currentDate)) {
                        occurrences++;

                        if (currentDate >= from) {
                            yield return currentDate;
                        }
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
}