using FluentAssertions;
using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrenceFilterBuilderTests {
        [Fact]
        public void Intersecting() {
            var recurrence = new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false);
            var builder = new RecurrenceFilterBuilder(new RecurrenceBuilder(), new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false));

            builder.Should().BeSameAs(builder.Intersecting(recurrence));

            builder.Recurrence.Should().BeSameAs(recurrence);
        }

        [Fact]
        public void BuildFilter() {
            var recurrence = new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false);
            var builder = new RecurrenceFilterBuilder(new RecurrenceBuilder(), recurrence);

            builder.BuildFilter().Should().BeOfType<RecurrenceFilter>().Which.Recurrence.Should().BeSameAs(recurrence);
        }
    }
}
