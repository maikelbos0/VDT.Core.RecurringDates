using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateRangeFilterTests {
        [Fact]
        public void Constructor_Without_StartDate_Sets_DateTime_MinValue() {
            var filter = new DateRangeFilter(null, new DateTime(2022, 1, 11));

            Assert.Equal(DateTime.MinValue, filter.StartDate);
        }

        [Fact]
        public void Constructor_Without_EndDate_Sets_DateTime_MaxValue() {
            var filter = new DateRangeFilter(new DateTime(2022, 1, 1), null);

            Assert.Equal(DateTime.MaxValue, filter.EndDate);
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-02", true)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", true)]
        [InlineData("2022-01-05", false)]
        public void IsFiltered(DateTime date, bool expectedIsFiltered) {
            var filter = new DateRangeFilter(new DateTime(2022, 1, 2), new DateTime(2022, 1, 4));

            Assert.Equal(expectedIsFiltered, filter.IsFiltered(date));
        }
    }
}
