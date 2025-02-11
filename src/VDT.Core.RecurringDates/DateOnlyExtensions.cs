#if NET8_0_OR_GREATER
using System;

namespace VDT.Core.RecurringDates;

internal static class DateOnlyExtensions {
    internal static DateTime ToDateTime(this DateOnly date) => new(date.DayNumber * TimeSpan.TicksPerDay);
}
#endif
