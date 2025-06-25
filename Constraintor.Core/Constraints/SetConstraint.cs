using System.Collections.Immutable;
using Constraintor.Core.Common;
using Constraintor.Core.Utils;

namespace Constraintor.Core.Constraints;

/// <summary>
/// Represents a constraint that limits field values to a specified set of allowed values.
/// Generic type supportive, as long as the type inherit from <see cref="IEquatable{T}"/>.
/// Thus,the values are expected to be convertible to their actual type.
/// Moreover, the process logic relying on preliminary stage of parsing in the appropriate DTO layer.
/// </summary>
public sealed class SetConstraint<T>(
    IEnumerable<string> targets,
    IReadOnlyDictionary<string, List<IActivationCondition>> when,
    IReadOnlyList<T> allowedValues)
    : Constraint(targets, when) where T : IEquatable<T>, IDeepCloneable<T>
{
    /// <summary>
    /// A collection of optional valid values for the field within the constraint.
    /// </summary>
    public readonly ImmutableHashSet<T?> AllowedSet =
        allowedValues
            .Select(v => v.DeepClone())
            .Where(c => c is not null)
            .ToImmutableHashSet();

    /// <summary>
    /// Activates constraint rules logic and validates the given field's value.
    /// In order to the constraint to be considered as satisfied,
    /// the value must be contained in <c>allowedValues</c> collection.
    /// </summary>
    /// <param name="fieldName">The target field name to be checked.</param>
    /// <param name="value">The value of the given field.</param>
    /// <returns>True if value satisfies constraint, false otherwise.</returns>
    protected override bool IsFieldValid(string fieldName, object? value)
        => value is T t && AllowedSet.Contains(t);
}