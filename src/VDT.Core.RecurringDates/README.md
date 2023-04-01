# VDT.Core.RecurringDates

Easily calculate ranges of recurring dates based on patterns in an intuitive and flexible way.

## Features

- Full range of patterns for calculating valid dates:
  - Daily, with weekend handling options
  - Weekly, with options for first day of the week and which days of the week are valid
  - Montly, either for days of the month or specific days of the week in a month
- Ability to combine patterns easily
- Filters to exclude dates from patterns
- Easy to use builder syntax
- Fully customizable and extensible: it's easy to add your own patterns and filters

## Builder syntax

Builders are provided to help you easily set up a recurrence with patterns to calculate dates. Use the static `Recurs` class as a starting point to create a
`RecurrenceBuilder` that lets you fluently build your recurrence:

- `From(startDate)` sets a start date
- `Until(endDate)` sets an end date
- `StopAfter(occurrences)` sets a maximum number of occurrences
- `Daily()` adds a pattern that repeats every day; it returns a builder that allows you to configure the day-based pattern
- `Every(interval).Days()` adds a pattern that repeats every `interval` days; it returns a builder that allows you to configure the day-based pattern
- `Weekly()` adds a pattern that repeats every week; it returns a builder that allows you to configure the week-based pattern
- `Every(interval).Weeks()` adds a pattern that repeats every `interval` weeks; it returns a builder that allows you to configure the week-based pattern
- `Monthly()` adds a pattern that repeats every month; it returns a builder that allows you to configure the month-based pattern
- `Every(interval).Months()` adds a pattern that repeats every `interval` months; it returns a builder that allows you to configure the month-based pattern
- `WithDateCaching()` enables caching of date validity which helps performance but increases memory usage
- `ExceptOn(dates)` adds a filter that invalidates the specified `dates`
- `ExceptFrom(startDate)` adds a filter that invalidates all dates from `startDate`
- `ExceptUntil(endDate)` adds a filter that invalidates all dates up to `endDate`
- `ExceptBetween(startDate, endDate)` adds a filter that invalidates all dates between `startDate` and `endDate`

The pattern builders for days, weeks and months in turn allow you to configure them as desired:

- Daily patterns can be provided with:
  - A reference date to determine what day the pattern starts in case if an interval greater than 1
  - The option to include weekends, skip weekends or move any date that falls in a weekend to the following Monday or preceding Friday
- Weekly patterns can be provided with:
  - A reference date and first day of the week to determine what week the pattern starts in case if an interval greater than 1
  - The days of the week that are valid
  - If no days of the week are provided the day of the week of the reference date will be used
- Monthly patterns can be provided with:
  - A reference date to determine what month the pattern starts in case if an interval greater than 1
  - The days of the month that are valid
  - The ordinal days of the week in the month that are valid (e.g. last Friday of the month)
  - The last days of the month that are valid (e.g. second last day of the month)
  - If no days are provided, the day of the month of the reference date will be used
  - The option to enable caching to speed up this relatively slow pattern at a cost of increased memory usage

The filter builders also allow some configuration after adding them:

- A date filter can be provided with addditional dates to filter
- A date range filter can be provided with a new start and end date

It's simple to chain calls to the above methods to set the limits and add multiple patterns for a single recurrence.

### Examples

```
// Build a recurrence that repeats every 9 days from January 1st 2022
var every9days = Recurs
    .Every(9).Days()
    .From(new DateTime(2022, 1, 1))
    .Build();

// Get all valid days for February 2022: 2022-02-06, 2022-02-15 and 2022-02-24
var dates = every9days.GetDates(new DateTime(2022, 2, 1), new DateTime(2022, 2, 28));


// Build a recurrence for the year 2022 with 2 patterns:
// - Every 2 weeks on Monday and Tuesday, determining the ongoing week using Sunday first day of the week
// - Every 1st and 3rd day of the month
var doublePattern = Recurs
    .From(new DateTime(2022, 1, 1))
    .Until(new DateTime(2022, 12, 31))
    .Every(2).Weeks().On(DayOfWeek.Monday, DayOfWeek.Tuesday).UsingFirstDayOfWeek(DayOfWeek.Sunday)
    .Monthly().On(1, 3)
    .Build();

// Get all valid days for February 2022: 2022-02-01, 2022-02-03, 2022-02-07, 2022-02-08, 2022-02-22 and 2022-02-23
var dates = doublePattern.GetDates(new DateTime(2022, 2, 1), new DateTime(2022, 2, 28));


// Build a recurrence that repeats every last Friday of the month for 3 occurrences, starting in April 2022
var fridays = Recurs
    .Monthly().On(DayOfWeekInMonth.Last, DayOfWeek.Friday)
    .From(new DateTime(2022, 4, 1))
    .StopAfter(3)
    .Build();

// Get all valid dates: 2022-04-29, 2022-05-27 and 2022-06-24
var dates = fridays.GetDates();


// Build a recurrence that repeats every weekday in December 2022, except for Christmas
var workingDays = Recurs
    .Daily()
    .SkipWeekends()
    .From(new DateTime(2022, 12, 1))
    .Until(new DateTime(2022, 12, 31))
    .ExceptOn(new DateTime(2022, 12, 25), new DateTime(2022, 12, 26))
    .Build();

// Get all valid dates: all week days in December except Christmas
var dates = workingDays.GetDates();
```

## Manual setup

Though the builder syntax makes it easy to write understandable recurrences, it is also possible to write recurrences and patterns by constructing the 
recurrence objects directly.

### Example

```
// Create a recurrence that repeats every 9 days from January 1st 2022
var every9days = new Recurrence(
    startDate: new DateTime(2022, 1, 1),
    endDate: DateTime.MaxValue,
    occurrences: null,
    patterns: new[] { new DailyRecurrencePattern(interval: 9, referenceDate: new DateTime(2022, 1, 1), weekendHandling: null) },
    filters: Enumerable.Empty<IFilter>(),
    cacheDates: false
);

// Get all valid days for February 2022: 2022-02-06, 2022-02-15 and 2022-02-24
var dates = every9days.GetDates(new DateTime(2022, 2, 1), new DateTime(2022, 2, 28));


// Create a recurrence for the year 2022 with 2 patterns:
// - Every 2 weeks on Monday and Tuesday, determining the ongoing week using Sunday first day of the week
// - Every 1st and 3rd day of the month
var doublePattern = new Recurrence(
    startDate: new DateTime(2022, 1, 1),
    endDate: new DateTime(2022, 12, 31),
    occurrences: null,
    patterns: new RecurrencePattern[] {
        new WeeklyRecurrencePattern(interval: 2, referenceDate: new DateTime(2022, 1, 1), firstDayOfWeek: DayOfWeek.Sunday, daysOfWeek: new[] { DayOfWeek.Monday, DayOfWeek.Tuesday }),
        new MonthlyRecurrencePattern(interval: 1, referenceDate: new DateTime(2022, 1, 1), daysOfMonth: new[] { 1, 3 })
    },
    filters: Enumerable.Empty<IFilter>(),
    cacheDates: false
);

// Get all valid days for February 2022: 2022-02-01, 2022-02-03, 2022-02-07, 2022-02-08, 2022-02-22 and 2022-02-23
var dates = doublePattern.GetDates(new DateTime(2022, 2, 1), new DateTime(2022, 2, 28));


// Create a recurrence that repeats every last Friday of the month for 3 occurrences, starting in April 2022
var fridays = new Recurrence(
    startDate: new DateTime(2022, 4, 1),
    endDate: DateTime.MaxValue,
    occurrences: 3,
    patterns: new[] { new MonthlyRecurrencePattern(interval: 1, referenceDate: new DateTime(2022, 4, 1), daysOfWeek: new[] { (DayOfWeekInMonth.Last, DayOfWeek.Friday) }) },
    filters: Enumerable.Empty<IFilter>(),
    cacheDates: false
);

// Get all valid dates: 2022-04-29, 2022-05-27 and 2022-06-24
var dates = fridays.GetDates();


// Create a recurrence that repeats every weekday in December 2022, except for Christmas
var workingDays = new Recurrence(
    startDate: new DateTime(2022, 12, 1),
    endDate: new DateTime(2022, 12, 31),
    occurrences: null,
    patterns: new[] { new DailyRecurrencePattern(interval: 1, weekendHandling: RecurrencePatternWeekendHandling.Skip) },
    filters: new[] { new DateFilter(new DateTime(2022, 12, 25), new DateTime(2022, 12, 26)) },
    cacheDates: false
);

// Get all valid dates: all week days in December except Christmas
var dates = workingDays.GetDates();
```

## Custom patterns

If you have a need for adding custom patterns, you can create your own by inheriting from the base `RecurrencePattern` class. In addition, you can create your
own builder by inheriting from the base `RecurrencePatternBuilder<TBuilder>` and adding extension methods to the `RecurrenceBuilder` interface and 
`RecurrencePatternBuilderStart` class.

Unfortunately compiler restrictions do not allow you to extend the static `Recurs` class, so you'll need to either manually instantiate the `RecurrenceBuilder`
class, start with one of the existing starting methods on the `Recurs` class, or create your own starting point.

### Example

```
public class AnnualRecurrencePattern : RecurrencePattern {
    public ImmutableHashSet<int> DaysOfYear { get; }

    public AnnualRecurrencePattern(int interval, DateTime? referenceDate, IEnumerable<int> daysOfYear) : base(interval, referenceDate) {
        DaysOfYear = ImmutableHashSet.CreateRange(daysOfYear);
    }

    public override bool IsValid(DateTime date)
        => DaysOfYear.Contains(date.DayOfYear) && (Interval == 1 || (date.Year - ReferenceDate.Year) % Interval == 0);
}

public class AnnualRecurrencePatternBuilder : RecurrencePatternBuilder<AnnualRecurrencePatternBuilder> {
    public List<int> DaysOfYear { get; }

    public AnnualRecurrencePatternBuilder(RecurrenceBuilder recurrenceBuilder, int interval) : base(recurrenceBuilder, interval) { }

    public AnnualRecurrencePatternBuilder On(params int[] days)
        => On(days.AsEnumerable());

    public AnnualRecurrencePatternBuilder On(IEnumerable<int> days) {
        DaysOfYear.AddRange(days);
        return this;
    }

    public override RecurrencePattern BuildPattern()
        => new AnnualRecurrencePattern(Interval, ReferenceDate ?? RecurrenceBuilder.StartDate, DaysOfYear);
}

public static class RecurrenceBuilderExtensions {
    public static AnnualRecurrencePatternBuilder Anually(this IRecurrenceBuilder recurrenceBuilder) {
        var builder = new AnnualRecurrencePatternBuilder(recurrenceBuilder.GetRecurrenceBuilder(), 1);
        recurrenceBuilder.GetRecurrenceBuilder().PatternBuilders.Add(builder);
        return builder;
    }
}

public static class RecurrencePatternBuilderStartExtensions {
    public static AnnualRecurrencePatternBuilder Years(this RecurrencePatternBuilderStart start) {
        var builder = new AnnualRecurrencePatternBuilder(start.RecurrenceBuilder, start.Interval);
        start.RecurrenceBuilder.PatternBuilders.Add(builder);
        return builder;
    }
}
```

```
// Create a recurrence that repeats every 2 years on day 100 and every year on day 300, starting in 2010
var recurrence = Recurs
    .Every(2).Years().On(100)
    .Anually().On(300)
    .From(new DateTime(2010, 1, 1))
    .Build();

// Get all valid days for the years 2010 to 2012; 2010-04-10, 2010-10-27, 2011-10-27, 2012-04-09 and 2012-10-26
var dates = recurrence.GetDates(new DateTime(2010, 1, 1), new DateTime(2012, 12, 31)).ToList();
```

## Custom filters

If you need custom filtering functionality this is easily achieved by implementing the <code>IFilter</code> interface. To create your own filter builder you
can inherit from the base <code>FilterBuilder</code> class and add extension methods to the <code>IRecurrenceBuilder</code> interface.

Unfortunately compiler restrictions do not allow you to extend the static `Recurs` class, so you'll need to either manually instantiate the `RecurrenceBuilder`
class, start with one of the existing starting methods on the `Recurs` class, or create your own starting point.

### Example

```
public class WeekdayFilter : IFilter {
    public ImmutableHashSet<DayOfWeek> DaysOfWeek { get; }

    public WeekdayFilter(IEnumerable<DayOfWeek> daysOfWeek) {
        DaysOfWeek = ImmutableHashSet.CreateRange(daysOfWeek);
    }

    public bool IsFiltered(DateTime date) => DaysOfWeek.Contains(date.DayOfWeek);
}

public class WeekdayFilterBuilder : FilterBuilder {
    public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();

    public WeekdayFilterBuilder(RecurrenceBuilder recurrenceBuilder) : base(recurrenceBuilder) { }

    public WeekdayFilterBuilder On(params DayOfWeek[] days)
        => On(days.AsEnumerable());

    public WeekdayFilterBuilder On(IEnumerable<DayOfWeek> days) {
        DaysOfWeek.AddRange(days);
        return this;
    }

    public override IFilter BuildFilter() => new WeekdayFilter(DaysOfWeek);
}

public static class RecurrenceBuilderExtensions {
    public static WeekdayFilterBuilder ExceptOn(this IRecurrenceBuilder recurrenceBuilder, params DayOfWeek[] days)
        => recurrenceBuilder.ExceptOn(days.AsEnumerable());

    public static WeekdayFilterBuilder ExceptOn(this IRecurrenceBuilder recurrenceBuilder, IEnumerable<DayOfWeek> days) {
        var builder = new WeekdayFilterBuilder(recurrenceBuilder.GetRecurrenceBuilder()).On(days);
        recurrenceBuilder.GetRecurrenceBuilder().FilterBuilders.Add(builder);
        return builder;
    }
}
```

```
// Create a recurrence that repeats every day except on Wednesdays
var recurrence = Recurs
    .Daily()
    .ExceptOn(DayOfWeek.Wednesday)
    .Build();

// Get all valid days for the first week of 2023; 2022-01-01, 2022-01-02, 2022-01-03, 2022-01-05, 2022-01-06 and 2022-01-07
var dates = recurrence.GetDates(new DateTime(2010, 1, 1), new DateTime(2012, 12, 31)).ToList();
```