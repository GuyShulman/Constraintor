namespace Constraintor.Core.Domains;

public class SetDomain : Domain
{
    public HashSet<object> Values { get; }

    public SetDomain() => Values = [];

    public SetDomain(IEnumerable<object> values) => Values = [..values];

    public override IEnumerable<object> GetValues() => Values;

    public override bool Contains(object value) => Values.Contains(value);

    public override Domain Intersect(Domain other)
    {
        if (other is not SetDomain sd)
            throw new InvalidOperationException("Cannot intersect SetDomain with non-SetDomain");

        return new SetDomain(Values.Intersect(sd.Values));
    }

    public override string ToString() => $"{{{string.Join(", ", Values)}}}";
}