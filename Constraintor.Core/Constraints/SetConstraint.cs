using System.Text.Json.Serialization;

namespace Constraintor.Core.Constraints;

public class SetConstraint : Constraint
{
    private IReadOnlyList<object> AllowedValues { get; } = [];

    /// <summary>
    /// Default constructor required for deserialization.
    /// </summary>
    [JsonConstructor]
    public SetConstraint()
    {
    }

    public override bool IsSatisfied(Dictionary<string, object> fieldsAssignment)
    {
        if (!ShouldApply(fieldsAssignment))
            return true;

        foreach (var target in Targets)
        {
            if (!fieldsAssignment.TryGetValue(target, out var val) || !AllowedValues.Contains(val))
                return false;
        }

        return true;
    }
}