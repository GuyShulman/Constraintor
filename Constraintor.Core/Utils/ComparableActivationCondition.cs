using Constraintor.Core.Common;

namespace Constraintor.Core.Utils;

public sealed class ComparableActivationCondition<T> : IActivationCondition
    where T : IDeepCloneable<T> , IComparable<T>, IEquatable<T>
{
    public ActivationConditionOperator Operator { get; init; }
    public required T Value { get; init; }

    public bool Matches(object? assign)
    {
        if (assign is not T typed) return false;

        return Operator switch
        {
            ActivationConditionOperator.Equals => Value.Equals(typed),
            ActivationConditionOperator.NotEquals => !Value.Equals(typed),
            ActivationConditionOperator.GreaterThan => typed.CompareTo(Value) > 0,
            ActivationConditionOperator.GreaterThanOrEqual => typed.CompareTo(Value) >= 0,
            ActivationConditionOperator.LessThan => typed.CompareTo(Value) < 0,
            ActivationConditionOperator.LessThanOrEqual => typed.CompareTo(Value) <= 0,
            _ => throw new NotSupportedException($"Operator '{Operator}' not supported for type '{typeof(T).Name}'")
        };
    }

    public IActivationCondition DeepClone() =>
        new ComparableActivationCondition<T>
        {
            Operator = Operator,
            Value = Value.DeepClone()
        };

    public override string ToString() => $"{Operator} {Value}";
}
