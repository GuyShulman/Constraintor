namespace Constraintor.Core.Domains;

public abstract class Domain
{
    public abstract IEnumerable<object>? GetValues();
    public abstract bool Contains(object value);
    public abstract Domain Intersect(Domain other);
}