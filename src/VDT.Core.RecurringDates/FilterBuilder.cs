using System;

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
        public RecurrenceBuilder GetRecurrenceBuilder() => RecurrenceBuilder;

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
        public Recurrence Build() => RecurrenceBuilder.Build();
    }
}
