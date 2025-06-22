namespace Constraintor.Core.Domains;

public class RangeDomain : Domain
{
    public int Min { get; }
    public int Max { get; }

    public RangeDomain(int min, int max)
    {
        if (min > max)
            throw new ArgumentException("Min must be less than or equal to Max");
        Min = min;
        Max = max;
    }

    public override IEnumerable<object> GetValues()
    {
        for (var i = Min; i <= Max; i++)
            yield return i;
    }

    public override bool Contains(object value)
    {
        if (value is int intValue)
            return intValue >= Min && intValue <= Max;
        return false;
    }

    public override Domain Intersect(Domain other)
    {
        if (other is not RangeDomain rd)
            throw new InvalidOperationException("Cannot intersect RangeDomain with non-RangeDomain");

        var newMin = Math.Max(Min, rd.Min);
        var newMax = Math.Min(Max, rd.Max);
        return newMin <= newMax
            ? new RangeDomain(newMin, newMax)
            : new SetDomain(); // empty domain
    }

    public override string ToString() => $"[{Min}–{Max}]";
}