using Constraintor.Core.Common;

namespace Constraintor.Core.Utils;

public sealed class InListActivationCondition<T> : IActivationCondition where T : IDeepCloneable<T>, IEquatable<T>
{
    public ActivationConditionOperator Operator { get; init; }
    public required HashSet<T> Values { get; init; }

    public bool Matches(object? assign)
    {
        if (assign is not T typed) return false;

        return Operator switch
        {
            ActivationConditionOperator.In => Values.Contains(typed),
            ActivationConditionOperator.NotIn => !Values.Contains(typed),
            _ => throw new NotSupportedException($"Unsupported operator: {Operator}")
        };
    }

    public IActivationCondition DeepClone() =>
        new InListActivationCondition<T>
        {
            Values = [..Values.Select(v => v.DeepClone())],
            Operator = Operator
        };

    public override string ToString() =>
        $"{Operator}, [{string.Join(", ", Values)}]";
}