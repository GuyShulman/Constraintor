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
    /// <summary>
    /// Activates constraint rules logic and validates the given field's value.
    /// In order to the constraint to be considered as satisfied, the value must be at the defined range.
    /// </summary>
    /// <param name="fieldName">The target field name to be checked.</param>
    /// <param name="value">The value of the given field.</param>
    /// <returns>True if value satisfies constraint, false otherwise.</returns>
    protected override bool IsFieldValid(string fieldName, object? value)
    {
        if (value is not T tValue) return false;

        if (min is not null)
        {
            var cmpLower = tValue.CompareTo(min);
            if (lowerInclusive ? cmpLower < 0 : cmpLower <= 0)
                return false;
        }

        if (max is null) return true;

        var cmpUpper = tValue.CompareTo(max);
        return upperInclusive ? cmpUpper <= 0 : cmpUpper < 0;
    }
}