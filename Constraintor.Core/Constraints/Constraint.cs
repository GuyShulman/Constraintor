using Constraintor.Core.Utils;

namespace Constraintor.Core.Constraints;

/// <summary>
/// Represents an abstract constraint on one or more fields in an object.
/// Each constraint may define optional conditions via <c>When</c> that determine
/// when it becomes active.
/// </summary>
public abstract class Constraint(
    IEnumerable<string> targets,
    IReadOnlyDictionary<string, List<ActivationCondition>>? when = null) : IViolatingConstraint
{
    /// <summary>
    /// The list of field names this constraint applies to.
    /// Typically, a constraint targets a single field, but multi-field constraints are allowed.
    /// </summary>
    public IReadOnlyList<string> Targets => targets.ToList().AsReadOnly();

    /// <summary>
    /// Prerequisite conditions under which this constraint becomes active.
    /// Each key represents a field that this constraint depends on,
    /// and the corresponding list defines the conditions that must be
    /// satisfied for the constraint to become active.
    /// All conditions across all fields must match (logical AND).
    /// Multiple conditions for a single field are also evaluated as AND.
    /// </summary>
    private IReadOnlyDictionary<string, List<ActivationCondition>> When { get; } =
        when ?? new Dictionary<string, List<ActivationCondition>>();

    /// <summary>
    /// Determines whether this constraint should be applied
    /// based on whether its activation conditions are met.
    /// </summary>
    /// <param name="fieldsAssignment">The current field-value mapping.</param>
    /// <returns><c>true</c> if the constraint is active; otherwise, <c>false</c>.</returns>
    private bool ShouldApply(IReadOnlyDictionary<string, object?> fieldsAssignment)
    {
        foreach (var (field, conditions) in When)
        {
            if (!fieldsAssignment.TryGetValue(field, out var value)) return false;
            if (conditions.Any(condition => !condition.Matches(value))) return false;
        }

        return true;
    }

    /// <summary>
    /// Collects all target fields that violate the constraint.
    /// This method is only evaluated if <see cref="ShouldApply"/> returns <c>true</c>.
    /// </summary>
    /// <param name="fieldsAssignment">A map of current field names to their assigned values.</param>
    /// <returns>
    /// A dictionary where each key is a field name that violates the constraint,
    /// and the value is the offending value.
    /// </returns>
    public virtual IReadOnlyDictionary<string, object?> GetViolatingFields(
        IReadOnlyDictionary<string, object?> fieldsAssignment)
    {
        var violations = new Dictionary<string, object?>();

        if (!ShouldApply(fieldsAssignment))
            return violations;

        foreach (var target in Targets)
        {
            if (!fieldsAssignment.TryGetValue(target, out var value)) continue;

            if (!IsFieldValid(target, value))
                violations[target] = value;
        }

        return violations;
    }

    /// <summary>
    /// Activates constraint rules logic and validates the given field's value.
    /// </summary>
    /// <param name="fieldName">The target field name to be checked.</param>
    /// <param name="value">The value of the given field.</param>
    /// <returns>True if value satisfies constraint, false otherwise.</returns>
    protected abstract bool IsFieldValid(string fieldName, object? value);

    public override string ToString() => $"{GetType().Name}: targets=[{string.Join(", ", Targets)}]";
}