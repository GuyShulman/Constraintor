using Constraintor.Core.Utils;

namespace Constraintor.Core.Constraints;

/// <summary>
/// Represents a constraint that limits field values to a specified set of allowed values.
/// Generic type supportive, as long as the type inherit from <see cref="IEquatable{T}"/>.
/// Thus,the values are expected to be convertible to their actual type.
/// Moreover, the process logic relying on preliminary stage of parsing in the appropriate DTO layer.
/// </summary>
public class SetConstraint<T>(
    IEnumerable<string> targets,
    IReadOnlyDictionary<string, List<ActivationCondition>> when,
    IReadOnlyList<T> allowedValues)
    : Constraint(targets, when) where T : IEquatable<T>
{
    /// <summary>
    /// A collection of optional valid values for the field within the constraint.
    /// </summary>
    public IReadOnlySet<T> AllowedSet => allowedValues.ToHashSet();

    /// <summary>
    ///  Determines whether the constraint is satisfied based on the current field assignments.
    /// Constraint considers satisfied if there is a match between the assignment and in <c>_allowedSet</c> collection.
    /// </summary>
    /// <param name="fieldsAssignment">The current field-value mapping.</param>
    /// <returns><c>true</c> if the constraint is satisfied, otherwise <c>false</c>.</returns>
    public override bool IsSatisfied(IReadOnlyDictionary<string, object?> fieldsAssignment)
    {
        if (!ShouldApply(fieldsAssignment))
            return true;

        foreach (var target in Targets)
        {
            if (!fieldsAssignment.TryGetValue(target, out var val) || val is not T typed ||
                !AllowedSet.Contains(typed))
                return false;
        }

        return true;
    }
}