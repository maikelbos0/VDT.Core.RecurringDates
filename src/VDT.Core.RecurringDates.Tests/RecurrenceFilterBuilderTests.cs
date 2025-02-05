using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class RecurrenceFilterBuilderTests {
        [Fact]
        public void Intersecting() {
            var recurrence = new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false);
            var builder = new RecurrenceFilterBuilder(new RecurrenceBuilder(), new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false));

            Assert.Same(builder, builder.Intersecting(recurrence));

            Assert.Same(recurrence, builder.Recurrence);
        }

        [Fact]
        public void BuildFilter() {
            var recurrence = new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false);
            var builder = new RecurrenceFilterBuilder(new RecurrenceBuilder(), recurrence);

            var result = Assert.IsType<RecurrenceFilter>(builder.BuildFilter());

            Assert.Same(recurrence, result.Recurrence);
        }
    }
}
