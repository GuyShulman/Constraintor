using Constraintor.Core.Common;

namespace Constraintor.Core.Constraints;

/// <summary>
/// Represents a constraint which enforces whether specific fields must be null or non-null individually.
/// multi-field Supportive via inputted map <c>targetFieldNullabilityMap</c>
/// indicates each field nullability under the conditions declared.
/// </summary>
public sealed class NullabilityConstraint(
    IDictionary<string, bool> targetNullability,
    IReadOnlyDictionary<string, List<IActivationCondition>>? when = null)
    : Constraint(targetNullability.Keys, when)
{
    /// <summary>
    /// Maps each field to a boolean indicating whether it must be null (true) or non-null (false).
    /// </summary>
    public readonly IReadOnlyDictionary<string, bool> TargetNullability =
        new Dictionary<string, bool>(targetNullability);


    /// <summary>
    /// Activates constraint rules logic and validates the given field's value.
    /// In order to the constraint to be considered as satisfied,
    /// the value's nullability must match to the constraint's defined configuration.
    /// </summary>
    /// <param name="fieldName">The target field name to be checked.</param>
    /// <param name="value">The value of the given field.</param>
    /// <returns>True if value satisfies constraint, false otherwise.</returns>
    protected override bool IsFieldValid(string fieldName, object? value)
    {
        var nullable = TargetNullability.TryGetValue(fieldName, out var rule) && rule;

        return nullable
            ? value is null
            : value is not null;
    }
}