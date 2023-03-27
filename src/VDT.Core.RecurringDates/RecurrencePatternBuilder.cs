using System;
using System.Collections.Generic;

namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Base builder for composing patterns for recurring dates
    /// </summary>
    public abstract class RecurrencePatternBuilder : IRecurrenceBuilder {
        /// <summary>
        /// Builder for date recurrences to which this pattern builder belongs
        /// </summary>
        public RecurrenceBuilder RecurrenceBuilder { get; }

        /// <summary>
        /// Interval between occurrences of the pattern to be created
        /// </summary>
        public int Interval { get; }

        /// <summary>
        /// Create a builder for composing patterns for dates
        /// </summary>
        /// <param name="recurrenceBuilder">Builder for date recurrences to which this pattern builder belongs</param>
        /// <param name="interval">Interval between occurrences of the pattern to be created</param>
        protected RecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) {
            RecurrenceBuilder = recurrenceBuilder;
            Interval = Guard.IsPositive(interval);
        }

        /// <summary>
        /// Build the pattern based on the provided specifications
        /// </summary>
        /// <returns>The composed recurring date pattern</returns>
        public abstract RecurrencePattern BuildPattern();

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

        /// <inheritdoc/>
        public DateFilterBuilder ExceptOn(params DateTime[] dates) => RecurrenceBuilder.ExceptOn(dates);

        /// <inheritdoc/>
        public DateFilterBuilder ExceptOn(IEnumerable<DateTime> dates) => RecurrenceBuilder.ExceptOn(dates);

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptStartingOn(DateTime? startDate) => RecurrenceBuilder.ExceptStartingOn(startDate);

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptEndingOn(DateTime? endDate) => RecurrenceBuilder.ExceptEndingOn(endDate);

        /// <inheritdoc/>
        public DateRangeFilterBuilder ExceptBetween(DateTime? startDate, DateTime? endDate) => RecurrenceBuilder.ExceptBetween(startDate, endDate);
    }

    /// <summary>
    /// Base builder for composing patterns for recurring dates
    /// </summary>
    /// <typeparam name="TBuilder">Builder implementation type</typeparam>
    public abstract class RecurrencePatternBuilder<TBuilder> : RecurrencePatternBuilder where TBuilder : RecurrencePatternBuilder<TBuilder> {
        /// <summary>
        /// Gets or sets the date to use as a reference for calculating periods when the interval is greater than 1; defaults to <see cref="RecurrenceBuilder.StartDate"/>
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

        /// <summary>
        /// Create a builder for composing patterns for dates
        /// </summary>
        /// <param name="recurrenceBuilder">Builder for date recurrences to which this pattern builder belongs</param>
        /// <param name="interval">Interval between occurrences of the pattern to be created</param>
        public RecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) : base(recurrenceBuilder, interval) { }

        /// <summary>
        /// Sets the date to use as a reference for calculating periods when the interval is greater than 1
        /// </summary>
        /// <param name="referenceDate">Reference date; defaults to <see cref="RecurrenceBuilder.StartDate"/></param>
        /// <returns>A reference to this recurrence pattern builder</returns>
        public TBuilder WithReferenceDate(DateTime? referenceDate) {
            ReferenceDate = referenceDate;
            return (TBuilder)this;
        }
    }
}