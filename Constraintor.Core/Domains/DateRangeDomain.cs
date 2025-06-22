namespace Constraintor.Core.Domains;

public class DateRangeDomain : Domain
{
    public DateTime Min { get; }
    public DateTime Max { get; }

    public DateRangeDomain(DateTime min, DateTime max)
    {
        if (min > max)
            throw new ArgumentException("Min must be before or equal to Max");

        Min = min;
        Max = max;
    }

    public override IEnumerable<object> GetValues()
    {
        var current = Min;
        while (current <= Max)
        {
            yield return current;
            current = current.AddDays(1); // Step granularity: 1 day
        }
    }

    public override bool Contains(object value) => value is DateTime dt && dt >= Min && dt <= Max;

    public override Domain Intersect(Domain other)
    {
        if (other is not DateRangeDomain drd)
            throw new InvalidOperationException("Cannot intersect DateRangeDomain with non-DateRangeDomain");

        var newMin = (Min > drd.Min) ? Min : drd.Min;
        var newMax = (Max < drd.Max) ? Max : drd.Max;

        return newMin <= newMax
            ? new DateRangeDomain(newMin, newMax)
            : new SetDomain(); // empty
    }

    public override string ToString() => $"DateRange: {Min:yyyy-MM-dd} to {Max:yyyy-MM-dd}";
}