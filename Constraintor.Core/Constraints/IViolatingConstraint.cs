namespace Constraintor.Core.Constraints;

public interface IViolatingConstraint
{
    IReadOnlyDictionary<string, object?> GetViolatingFields(IReadOnlyDictionary<string, object?> fieldsAssignment);
}