using Constraintor.Core.Utils;

namespace Constraintor.Core.Constraints;

/// <summary>
/// Represents an abstract constraint on one or more fields in an object.
/// Each constraint may define optional conditions via <c>When</c> that determine
/// when it becomes active.
/// </summary>
public abstract class Constraint(
    IEnumerable<string> targets,
    IReadOnlyDictionary<string, List<ActivationCondition>>? when = null)
{
    /// <summary>
    /// The list of field names this constraint applies to.
    /// Typically, a constraint targets a single field, but multi-field constraints are allowed.
    /// </summary>
    protected IReadOnlyList<string> Targets { get; } = targets.ToList().AsReadOnly();

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
    ///  Determines whether the constraint is satisfied based on the current field assignments.
    /// This method is only evaluated if <see cref="ShouldApply"/> returns true.
    /// </summary>
    /// <param name="fieldsAssignment">The current field-value mapping.</param>
    /// <returns><c>true</c> if the constraint is satisfied, otherwise <c>false</c>.</returns>
    public abstract bool IsSatisfied(IReadOnlyDictionary<string, object?> fieldsAssignment);

    /// <summary>
    /// Determines whether this constraint should be applied
    /// based on whether its activation conditions are met.
    /// </summary>
    /// <param name="fieldsAssignment">The current field-value mapping.</param>
    /// <returns><c>true</c> if the constraint is active; otherwise, <c>false</c>.</returns>
    protected bool ShouldApply(IReadOnlyDictionary<string, object?> fieldsAssignment)
    {
        foreach (var (field, conditions) in When)
        {
            if (!fieldsAssignment.TryGetValue(field, out var value))
                return false;

            if (conditions.Any(condition => !condition.Matches(value)))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString() => $"{GetType().Name}: targets=[{string.Join(", ", Targets)}]";
}