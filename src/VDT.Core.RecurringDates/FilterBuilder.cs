using System;
using System.Collections.Generic;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Base builder for composing recurrence filters
    /// </summary>
    public abstract class FilterBuilder : IRecurrenceBuilder {
        /// <summary>
        /// Builder for date recurrences to which this filter builder belongs
        /// </summary>
        public RecurrenceBuilder RecurrenceBuilder { get; }

        /// <summary>
        /// Create a builder for composing recurrence filters
        /// </summary>
        /// <param name="recurrenceBuilder">Builder for date recurrences to which this filter builder belongs</param>
        protected FilterBuilder(RecurrenceBuilder recurrenceBuilder) {
            RecurrenceBuilder = recurrenceBuilder;
        }

        /// <summary>
        /// Build the filter based on the provided specifications
        /// </summary>
        /// <returns>The composed recurrence filter</returns>
        public abstract IFilter BuildFilter();

        /// <inheritdoc/>
        public IRecurrenceBuilder From(DateTime? startDate) => RecurrenceBuilder.From(startDate);

        /// <inheritdoc/>
        public IRecurrenceBuilder Until(DateTime? endDate) => RecurrenceBuilder.Until(endDate);

        /// <inheritdoc/>
        public IRecurrenceBuilder StopAfter(int? occurrences) => RecurrenceBuilder.StopAfter(occurrences);

        /// <inheritdoc/>
        public DailyRecurrencePatternBuilder Daily() => RecurrenceBuilder.Daily();

        /// <inheritdoc/>
        public WeeklyRecurrencePatternBuilder Weekly() => RecurrenceBuilder.Weekly();

        /// <inheritdoc/>
        public MonthlyRecurrencePatternBuilder Monthly() => RecurrenceBuilder.Monthly();

        /// <inheritdoc/>
        public RecurrencePatternBuilderStart Every(int interval) => RecurrenceBuilder.Every(interval);

        /// <inheritdoc/>
        public RecurrenceBuilder WithDateCaching() => RecurrenceBuilder.WithDateCaching();

        /// <inheritdoc/>
        public DateFilterBuilder ExceptOn(params DateTime[] dates) => RecurrenceBuilder.ExceptOn(dates);

        /// <inheritdoc/>
        public DateFilterBuilder ExceptOn(IEnumerable<DateTime> dates) => RecurrenceBuilder.ExceptOn(dates);

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptFrom(DateTime? startDate) => RecurrenceBuilder.ExceptFrom(startDate);

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptUntil(DateTime? endDate) => RecurrenceBuilder.ExceptUntil(endDate);

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptBetween(DateTime? startDate, DateTime? endDate) => RecurrenceBuilder.ExceptBetween(startDate, endDate);

        /// <inheritdoc/>
        public Recurrence Build() => RecurrenceBuilder.Build();
    }
}
