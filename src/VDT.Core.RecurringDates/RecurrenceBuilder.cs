using System;
using System.Collections.Generic;
using System.Linq;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder for date recurrences
    /// </summary>
    public class RecurrenceBuilder : IRecurrenceBuilder {
        /// <summary>
        /// Gets or sets the inclusive start date for this recurrence; defaults to <see cref="DateTime.MinValue"/>
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the inclusive end date for this recurrence; defaults to <see cref="DateTime.MaxValue"/>
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of occurrences for this recurrence; if <see langword="null"/> it repeats without limit
        /// </summary>
        public int? Occurrences { get; set; }

        /// <summary>
        /// Indicates whether or not date validity should be cached; if you use custom patterns that can be edited the cache should not be enabled; defaults 
        /// to <see langword="false"/>
        /// </summary>
        public bool CacheDates { get; set; }

        /// <summary>
        /// Gets or sets the builders which will be invoked to build recurrence patterns for this recurrence
        /// </summary>
        public List<RecurrencePatternBuilder> PatternBuilders { get; set; } = new();

        /// <summary>
        /// Gets or sets the builders which will be invoked to build filters for this recurrence
        /// </summary>
        public List<FilterBuilder> FilterBuilders { get; set; } = new();

        /// <inheritdoc/>
        public IRecurrenceBuilder From(DateTime? startDate) {
            StartDate = startDate;
            return this;
        }

        /// <inheritdoc/>
        public IRecurrenceBuilder Until(DateTime? endDate) {
            EndDate = endDate;
            return this;
        }

        /// <inheritdoc/>
        public IRecurrenceBuilder StopAfter(int? occurrences) {
            Occurrences = occurrences;
            return this;
        }

        /// <inheritdoc/>
        public DailyRecurrencePatternBuilder Daily() {
            var builder = new DailyRecurrencePatternBuilder(this, 1);
            PatternBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public WeeklyRecurrencePatternBuilder Weekly() {
            var builder = new WeeklyRecurrencePatternBuilder(this, 1);
            PatternBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public MonthlyRecurrencePatternBuilder Monthly() {
            var builder = new MonthlyRecurrencePatternBuilder(this, 1);
            PatternBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public RecurrencePatternBuilderStart Every(int interval) {
            return new RecurrencePatternBuilderStart(this, interval);
        }

        /// <inheritdoc/>
        public RecurrenceBuilder WithDateCaching() {
            CacheDates = true;
            return this;
        }

        /// <inheritdoc/>
        public DateFilterBuilder ExceptOn(params DateTime[] dates) => ExceptOn(dates.AsEnumerable());

        /// <inheritdoc/>
        public DateFilterBuilder ExceptOn(IEnumerable<DateTime> dates) {
            var builder = new DateFilterBuilder(this) {
                Dates = dates.ToList()
            };
            FilterBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptFrom(DateTime? startDate) {
            var builder = new DateRangeFilterBuilder(this) {
                StartDate = startDate
            };
            FilterBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public RecurrenceFilterBuilder ExceptIntersecting(Recurrence recurrence) {
            var builder = new RecurrenceFilterBuilder(this, recurrence);
            FilterBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptUntil(DateTime? endDate) {
            var builder = new DateRangeFilterBuilder(this) {
                EndDate = endDate
            };
            FilterBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptBetween(DateTime? startDate, DateTime? endDate) {
            var builder = new DateRangeFilterBuilder(this) {
                StartDate = startDate,
                EndDate = endDate
            };
            FilterBuilders.Add(builder);
            return builder;
        }

        /// <inheritdoc/>
        public Recurrence Build() {
            return new Recurrence(StartDate, EndDate, Occurrences, PatternBuilders.Select(builder => builder.BuildPattern()), FilterBuilders.Select(builder => builder.BuildFilter()), CacheDates);
        }
    }
}