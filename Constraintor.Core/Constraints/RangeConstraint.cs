using Constraintor.Core.Utils;

namespace Constraintor.Core.Constraints;

public sealed class RangeConstraint<T>(
    IEnumerable<string> targets,
    IReadOnlyDictionary<string, List<ActivationCondition>> when,
    T? min,
    T? max,
    bool lowerInclusive = false,
    bool upperInclusive = false)
    : Constraint(targets, when)
    where T : IComparable<T>
{
    public override bool IsSatisfied(IReadOnlyDictionary<string, object?> fieldsAssignment)
    {
        if (!ShouldApply(fieldsAssignment))
            return true;

        foreach (var target in Targets)
        {
            if (!fieldsAssignment.TryGetValue(target, out var rawValue) || rawValue is not T value)
                continue;

            if (min is not null)
            {
                var cmp = value.CompareTo(min);
                if (lowerInclusive ? cmp < 0 : cmp <= 0)
                    return false;
            }

            if (max is null) continue;
            {
                var cmp = value.CompareTo(max);
                if (upperInclusive ? cmp > 0 : cmp >= 0)
                    return false;
            }
        }

        return true;
    }
}