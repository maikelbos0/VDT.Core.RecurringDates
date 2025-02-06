using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VDT.Core.RecurringDates.Tests {
    public class FilterBuilderTests {
        private class TestFilterBuilder : FilterBuilder {
            public TestFilterBuilder(RecurrenceBuilder recurrenceBuilder) : base(recurrenceBuilder) { }

            public override IFilter BuildFilter() => throw new NotImplementedException();
        }

        [Fact]
        public void From() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            Assert.Same(builder, filterBuilder.From(new DateTime(2022, 1, 1)));

            Assert.Equal(new DateTime(2022, 1, 1), builder.StartDate);
        }

        [Fact]
        public void Until() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            Assert.Same(builder, filterBuilder.Until(new DateTime(2022, 12, 31)));

            Assert.Equal(new DateTime(2022, 12, 31), builder.EndDate);
        }

        [Fact]
        public void StopAfter() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            Assert.Same(builder, filterBuilder.StopAfter(10));

            Assert.Equal(10, builder.Occurrences);
        }

        [Fact]
        public void Daily() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Daily();

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.PatternBuilders);
            Assert.Equal(1, result.Interval);
        }

        [Fact]
        public void Weekly() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Weekly();

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.PatternBuilders);
            Assert.Equal(1, result.Interval);
        }

        [Fact]
        public void Monthly() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Monthly();

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.PatternBuilders);
            Assert.Equal(1, result.Interval);
        }

        [Fact]
        public void Every() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Every(2);

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Equal(2, result.Interval);
        }

        [Fact]
        public void WithDateCaching() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            Assert.Same(builder, filterBuilder.WithDateCaching());
            Assert.True(builder.CacheDates);
        }

        [Fact]
        public void ExceptOn() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptOn(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.FilterBuilders);
            Assert.Equivalent(new List<DateTime>() { new DateTime(2022, 1, 1), new DateTime(2022, 1, 2) }, result.Dates);
        }

        [Fact]
        public void ExceptFrom() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptFrom(new DateTime(2022, 2, 3));

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.FilterBuilders);
            Assert.Equal(new DateTime(2022, 2, 3), result.StartDate);
        }

        [Fact]
        public void ExceptUntil() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptUntil(new DateTime(2022, 2, 5));

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.FilterBuilders);
            Assert.Equal(new DateTime(2022, 2, 5), result.EndDate);
        }

        [Fact]
        public void ExceptBetween() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.FilterBuilders);
            Assert.Equal(new DateTime(2022, 2, 3), result.StartDate);
            Assert.Equal(new DateTime(2022, 2, 5), result.EndDate);
        }

        [Fact]
        public void ExceptIntersecting() {
            var recurrence = new Recurrence(null, null, null, Enumerable.Empty<RecurrencePattern>(), Enumerable.Empty<IFilter>(), false);
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.ExceptIntersecting(recurrence);

            Assert.Same(builder, result.RecurrenceBuilder);
            Assert.Contains(result, builder.FilterBuilders);
            Assert.Same(recurrence, result.Recurrence);
        }

        [Fact]
        public void Build() {
            var builder = new RecurrenceBuilder();
            var filterBuilder = new TestFilterBuilder(builder);

            var result = filterBuilder.Build();

            Assert.NotNull(result);
        }
    }
}
