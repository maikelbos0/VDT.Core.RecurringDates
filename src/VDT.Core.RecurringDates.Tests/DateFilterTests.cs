using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateFilterTests {
        [Fact]
        public void Constructor_Removes_Time_From_Dates() {
            var filter = new DateFilter(new[] { new DateTime(2022, 1, 2, 11, 12, 30), new DateTime(2022, 1, 3, 12, 37, 30) });

            Assert.Contains(new DateTime(2022, 1, 2), filter.Dates);
            Assert.Contains(new DateTime(2022, 1, 3), filter.Dates);
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-02", true)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", false)]
        public void IsFiltered(DateTime date, bool expectedIsFiltered) {
            var filter = new DateFilter(new[] { new DateTime(2022, 1, 2), new DateTime(2022, 1, 3) });

            Assert.Equal(expectedIsFiltered, filter.IsFiltered(date));
        }
    }
}
