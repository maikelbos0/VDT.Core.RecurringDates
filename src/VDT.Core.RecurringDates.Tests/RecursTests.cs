﻿using System;
using Xunit;

namespace VDT.Core.RecurringDates.Tests;

public class RecursTests {
    [Fact]
    public void From() {
        var result = Recurs.From(new DateTime(2022, 1, 1));

        Assert.Equal(new DateTime(2022, 1, 1), Assert.IsType<RecurrenceBuilder>(result).StartDate);
    }

    [Fact]
    public void Until() {
        var result = Recurs.Until(new DateTime(2022, 12, 31));

        Assert.Equal(new DateTime(2022, 12, 31), Assert.IsType<RecurrenceBuilder>(result).EndDate);
    }

    [Fact]
    public void StopAfter() {
        var result = Recurs.StopAfter(10);

        Assert.Equal(10, Assert.IsType<RecurrenceBuilder>(result).Occurrences);
    }

    [Fact]
    public void Daily() {
        var result = Recurs.Daily();

        Assert.Equal(1, result.Interval);
    }

    [Fact]
    public void Weekly() {
        var result = Recurs.Weekly();

        Assert.Equal(1, result.Interval);
    }

    [Fact]
    public void Monthly() {
        var result = Recurs.Monthly();

        Assert.Equal(1, result.Interval);
    }

    [Fact]
    public void Every() {
        var result = Recurs.Every(2);

        Assert.Equal(2, result.Interval);
    }

    [Fact]
    public void WithDateCaching() {
        var result = Recurs.WithDateCaching();

        Assert.True(result.CacheDates);
    }

    [Fact]
    public void ExceptOn() {
        var result = Recurs.ExceptOn(new DateTime(2022, 1, 1), new DateTime(2022, 1, 2));

        Assert.Equal([new DateTime(2022, 1, 1), new DateTime(2022, 1, 2)], result.Dates);
    }

    [Fact]
    public void ExceptFrom() {
        var result = Recurs.ExceptFrom(new DateTime(2022, 2, 3));

        Assert.Equal(new DateTime(2022, 2, 3), result.StartDate);
    }

    [Fact]
    public void ExceptUntil() {
        var result = Recurs.ExceptUntil(new DateTime(2022, 2, 5));

        Assert.Equal(new DateTime(2022, 2, 5), result.EndDate);
    }

    [Fact]
    public void ExceptBetween() {
        var result = Recurs.ExceptBetween(new DateTime(2022, 2, 3), new DateTime(2022, 2, 5));

        Assert.Equal(new DateTime(2022, 2, 3), result.StartDate);
        Assert.Equal(new DateTime(2022, 2, 5), result.EndDate);
    }
}
