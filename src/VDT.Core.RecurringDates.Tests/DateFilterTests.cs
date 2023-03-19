using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateFilterTests {
        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-02", true)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", false)]
        public void IsFiltered(DateTime date, bool expectedIsFiltered) {
            var filter = new DateFilter(new DateTime(2022, 1, 2), new DateTime(2022, 1, 3));

            Assert.Equal(expectedIsFiltered, filter.IsFiltered(date));
        }
    }
}
