﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace VDT.Core.RecurringDates;

/// <summary>
/// Builder for composing patterns for dates that recur every week or every several weeks
/// </summary>
public class WeeklyRecurrencePatternBuilder : RecurrencePatternBuilder<WeeklyRecurrencePatternBuilder> {
    /// <summary>
    /// Gets or sets the first day of the week to use when calculating the reference week when the interval is greater than 1; defaults to
    /// <see cref="DateTimeFormatInfo.FirstDayOfWeek"/> from <see cref="Thread.CurrentCulture"/>
    /// </summary>
    public DayOfWeek? FirstDayOfWeek { get; set; }

    /// <summary>
    /// Gets or sets the days of the week which are valid for this recurrence pattern
    /// </summary>
    public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();

    /// <summary>
    /// Create a builder for composing patterns for dates that recur every week or every several weeks
    /// </summary>
    /// <param name="recurrenceBuilder">Builder for date recurrences to which this pattern builder belongs</param>
    /// <param name="interval">Interval in weeks between occurrences of the pattern to be created</param>
    public WeeklyRecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) : base(recurrenceBuilder, interval) { }

    /// <summary>
    /// Sets the first day of the week to use when calculating the reference week when the interval is greater than 1
    /// </summary>
    /// <param name="firstDayOfWeek">Day of the week to use as the first; defaults to <see cref="DateTimeFormatInfo.FirstDayOfWeek"/> from <see cref="Thread.CurrentCulture"/></param>
    /// <returns>A reference to this recurrence pattern builder</returns>
    public WeeklyRecurrencePatternBuilder UsingFirstDayOfWeek(DayOfWeek? firstDayOfWeek) {
        FirstDayOfWeek = firstDayOfWeek;
        return this;
    }

    /// <summary>
    /// Adds the given days of the week to the valid days of the week for this recurrence pattern
    /// </summary>
    /// <param name="days">Days of the week that should be added</param>
    /// <returns>A reference to this recurrence pattern builder</returns>
    public WeeklyRecurrencePatternBuilder On(params DayOfWeek[] days)
        => On(days.AsEnumerable());

    /// <summary>
    /// Adds the given days of the week to the valid days of the week for this recurrence pattern
    /// </summary>
    /// <param name="days">Days of the week that should be added</param>
    /// <returns>A reference to this recurrence pattern builder</returns>
    public WeeklyRecurrencePatternBuilder On(IEnumerable<DayOfWeek> days) {
        DaysOfWeek.AddRange(days);
        return this;
    }

    /// <inheritdoc/>
    public override RecurrencePattern BuildPattern()
        => new WeeklyRecurrencePattern(Interval, ReferenceDate ?? RecurrenceBuilder.StartDate, FirstDayOfWeek, DaysOfWeek);
}