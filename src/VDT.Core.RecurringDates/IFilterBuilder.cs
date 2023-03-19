namespace VDT.Core.RecurringDates {
    /// <summary>
    /// Builder interafce for composing recurrence filters
    /// </summary>
    public interface IFilterBuilder {
        /// <summary>
        /// Build the filter based on the provided specifications
        /// </summary>
        /// <returns>The composed recurrence filter</returns>
        IFilter BuildFilter();
    }
}
