using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrencePatternTests {
        private class TestRecurrencePattern : RecurrencePattern {
            public TestRecurrencePattern(int interval, DateTime? referenceDate) : base(interval, referenceDate) { }

            public override bool IsValid(DateTime date) => throw new NotImplementedException();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Constructor_Throws_For_Invalid_Interval(int interval) {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TestRecurrencePattern(interval, DateTime.MinValue));
        }

        [Fact]
        public void Constructor_Without_ReferenceDate_Sets_DateTime_MinValue() {
            var pattern = new TestRecurrencePattern(1, null);

            Assert.Equal(DateTime.MinValue, pattern.ReferenceDate);
        }
    }
}
