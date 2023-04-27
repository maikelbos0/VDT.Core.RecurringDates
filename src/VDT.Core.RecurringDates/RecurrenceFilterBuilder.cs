namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder for recurrence filters
    /// </summary>
    public class RecurrenceFilterBuilder : FilterBuilder {
        /// <summary>
        /// Gets or sets the recurrence to check for dates to invalidate
        /// </summary>
        public Recurrence Recurrence { get; set; }

        /// <summary>
        /// Create a builder for composing a filter to invalidate based on a recurrence
        /// </summary>
        /// <param name="recurrenceBuilder">Builder for date recurrences to which this filter builder belongs</param>
        /// <param name="recurrence">Recurrence to check for dates to invalidate</param>
        public RecurrenceFilterBuilder(RecurrenceBuilder recurrenceBuilder, Recurrence recurrence) : base(recurrenceBuilder) {
            Recurrence = recurrence;
        }

        /// <summary>
        /// Sets the recurrence to check for dates to invalidate
        /// </summary>
        /// <param name="recurrence">Recurrence to check for dates to invalidate</param>
        /// <returns>A reference to this filter builder</returns>
        public RecurrenceFilterBuilder Intersecting(Recurrence recurrence) {
            Recurrence = recurrence;
            return this;
        }

        /// <inheritdoc/>
        public override IFilter BuildFilter() => new RecurrenceFilter(Recurrence);
    }
}
