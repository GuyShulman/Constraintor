using System.Text.RegularExpressions;
using Constraintor.Core.Common;

namespace Constraintor.Core.Utils;

public sealed class StringActivationCondition : IActivationCondition
{
    public ActivationConditionOperator Operator { get; init; }
    public required string Value { get; init; }

    public bool Matches(object? assign)
    {
        if (assign is not string typed) return false;

        return Operator switch
        {
            ActivationConditionOperator.Equals => typed == Value,
            ActivationConditionOperator.NotEquals => typed != Value,
            ActivationConditionOperator.StartsWith => typed.StartsWith(Value),
            ActivationConditionOperator.EndsWith => typed.EndsWith(Value),
            ActivationConditionOperator.Matches => Regex.IsMatch(typed, Value),
            _ => throw new NotSupportedException($"Operator '{Operator}' is not valid for strings")
        };
    }

    public IActivationCondition DeepClone() =>
        new StringActivationCondition
        {
            Operator = Operator,
            Value = new string(Value)
        };

    public override string ToString() => $"{Operator} \"{Value}\"";
}