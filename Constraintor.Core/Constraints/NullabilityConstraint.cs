using Constraintor.Core.Utils;

namespace Constraintor.Core.Constraints;

/// <summary>
/// Represents a constraint which enforces whether specific fields must be null or non-null individually.
/// multi-field Supportive via inputted map <c>targetFieldNullabilityMap</c>
/// indicates each field nullability under the conditions declared.
/// </summary>
public sealed class NullabilityConstraint(
    IReadOnlyDictionary<string, bool> targetFieldNullabilityMap,
    IReadOnlyDictionary<string, List<ActivationCondition>>? when = null)
    : Constraint(targetFieldNullabilityMap.Keys, when)
{
    /// <summary>
    /// Maps each field to a boolean indicating whether it must be null (true) or non-null (false).
    /// </summary>
    private IReadOnlyDictionary<string, bool> TargetFieldNullabilityMap { get; } = targetFieldNullabilityMap;

    /// <summary>
    ///  Determines whether the constraint is satisfied based on the current field assignments.
    /// Constraint considers satisfied if there is a match between the assignment and in <c></c> nullability mapping declared.
    /// </summary>
    /// <param name="fieldsAssignment">The current field-value mapping.</param>
    /// <returns><c>true</c> if the constraint is satisfied, otherwise <c>false</c>.</returns>
    public override bool IsSatisfied(IReadOnlyDictionary<string, object?> fieldsAssignment)
    {
        if (!ShouldApply(fieldsAssignment))
            return true;

        foreach (var (field, nullable) in TargetFieldNullabilityMap)
        {
            var exists = fieldsAssignment.TryGetValue(field, out var value);

            if (nullable)
            {
                if (exists && value is not null)
                    return false;
            }
            else
            {
                if (!exists || value is null)
                    return false;
            }
        }

        return true;
    }
}