namespace Constraintor.Core.Constraints;

public class RangeConstraint : Constraint
{
    public int? Min { get; set; }
    public int? Max { get; set; }

    public override bool IsSatisfied(Dictionary<string, object> fieldsAssignment)
    {
        if (!ShouldApply(fieldsAssignment))
            return true;

        foreach (var target in Targets)
        {
            if (!fieldsAssignment.TryGetValue(target, out var val))
                continue;

            if (val is not int i)
                return false;

            if (Min.HasValue && i < Min.Value)
                return false;

            if (Max.HasValue && i > Max.Value)
                return false;
        }

        return true;
    }
}