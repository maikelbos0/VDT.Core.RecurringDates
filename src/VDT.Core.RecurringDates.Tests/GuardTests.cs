using FluentAssertions;
using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class GuardTests {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(int.MaxValue)]
        public void IsPositive_Returns_Positive_Numbers(int value) {
            Guard.IsPositive(value).Should().Be(value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsPositive_Throws_For_Negative_Numbers_Or_Zero(int value) {
            var action = () => Guard.IsPositive(value);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 9, 19)]
        [InlineData(int.MaxValue)]
        public void ArePositive_Returns_Positive_Numbers(params int[] values) {
            Guard.ArePositive(values).Should().BeEquivalentTo(values);
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(9, 19, -1)]
        [InlineData(int.MinValue)]
        public void ArePositive_Throws_For_Negative_Numbers_Or_Zero(params int[] values) {
            var action = () => Guard.ArePositive(values);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
