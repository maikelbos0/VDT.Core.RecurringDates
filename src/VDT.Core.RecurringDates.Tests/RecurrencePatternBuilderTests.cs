using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class RecurrencePatternBuilderTests {
    private class TestRecurrencePatternBuilder : RecurrencePatternBuilder<TestRecurrencePatternBuilder> {
        public TestRecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) : base(recurrenceBuilder, interval) { }

        public override RecurrencePattern BuildPattern() => throw new NotImplementedException();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Constructor_Throws_For_Invalid_Interval(int interval) {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TestRecurrencePatternBuilder(new RecurrenceBuilder(), interval));
    }

    [Fact]
    public void WithReferenceDate() {
        var builder = new TestRecurrencePatternBuilder(new RecurrenceBuilder(), 1);

        Assert.Same(builder, builder.WithReferenceDate(new DateTime(2022, 2, 1)));

        Assert.Equal(new DateTime(2022, 2, 1), builder.ReferenceDate);
    }

    [Fact]
    public void From() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        Assert.Same(builder, patternBuilder.From(new DateTime(2022, 1, 1)));

        Assert.Equal(new DateTime(2022, 1, 1), builder.StartDate);
    }

    [Fact]
    public void Until() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        Assert.Same(builder, patternBuilder.Until(new DateTime(2022, 12, 31)));

        Assert.Equal(new DateTime(2022, 12, 31), builder.EndDate);
    }

    [Fact]
    public void StopAfter() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        Assert.Same(builder, patternBuilder.StopAfter(10));

        Assert.Equal(10, builder.Occurrences);
    }

    [Fact]
    public void Daily() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.Daily();

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.PatternBuilders);
        Assert.Equal(1, result.Interval);
    }

    [Fact]
    public void Weekly() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.Weekly();

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.PatternBuilders);
        Assert.Equal(1, result.Interval);
    }

    [Fact]
    public void Monthly() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.Monthly();

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.PatternBuilders);
        Assert.Equal(1, result.Interval);
    }

    [Fact]
    public void Every() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.Every(2);

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Equal(2, result.Interval);
    }

    [Fact]
    public void WithDateCaching() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        Assert.Same(builder, patternBuilder.WithDateCaching());

        Assert.True(builder.CacheDates);
    }

    [Fact]
    public void ExceptOn() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.ExceptOn(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.FilterBuilders);
        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 2)], result.Dates);
    }

    [Fact]
    public void ExceptFrom() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.ExceptFrom(new DateTime(2022, 2, 3));

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.FilterBuilders);
        Assert.Equal(new DateTime(2022, 2, 3), result.StartDate);
    }

    [Fact]
    public void ExceptUntil() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.ExceptUntil(new DateTime(2022, 2, 5));

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.FilterBuilders);
        Assert.Equal(new DateTime(2022, 2, 5), result.EndDate);
    }

    [Fact]
    public void ExceptBetween() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.FilterBuilders);
        Assert.Equal(new DateTime(2022, 2, 3), result.StartDate);
        Assert.Equal(new DateTime(2022, 2, 5), result.EndDate);
    }

    [Fact]
    public void ExceptIntersecting() {
        var recurrence = new Recurrence(DateOnly.MinValue, DateOnly.MaxValue, null, [], [], false);
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.ExceptIntersecting(recurrence);

        Assert.Same(builder, result.RecurrenceBuilder);
        Assert.Contains(result, builder.FilterBuilders);
        Assert.Same(recurrence, result.Recurrence);
    }

    [Fact]
    public void Build() {
        var builder = new RecurrenceBuilder();
        var patternBuilder = new TestRecurrencePatternBuilder(builder, 1);

        var result = patternBuilder.Build();

        Assert.NotNull(result);
    }
}
