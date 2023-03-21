using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class DateFilterTests {
        [Fact]
        public void Constructor_Removes_Time_From_Dates() {
            var filter = new DateFilter(new[] { new DateTime(2022, 1, 2, 11, 12, 30), new DateTime(2022, 1, 3, 12, 37, 30) });

            filter.Dates.Should().BeEquivalentTo(new[] { new DateTime(2022, 1, 2), new DateTime(2022, 1, 3) });
        }

        [Theory]
        [InlineData("2022-01-01", false)]
        [InlineData("2022-01-02", true)]
        [InlineData("2022-01-03", true)]
        [InlineData("2022-01-04", false)]
        public void IsFiltered(DateTime date, bool expectedIsFiltered) {
            var filter = new DateFilter(new[] { new DateTime(2022, 1, 2), new DateTime(2022, 1, 3) });

            filter.IsFiltered(date).Should().Be(expectedIsFiltered);
        }
    }
}
