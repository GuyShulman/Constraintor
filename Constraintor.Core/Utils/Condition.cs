using System.Text.RegularExpressions;

namespace Constraintor.Core.Utils;

public enum ActivationConditionOperator
{
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    In,
    NotIn,
    StartsWith,
    EndsWith,
    Matches // Regex
}

public class ActivationCondition
{
    public ActivationConditionOperator Operator { get; set; }
    public required object Value { get; set; }

    public bool Matches(object? assign)
    {
        return assign != null && Operator switch
        {
            ActivationConditionOperator.Equals => Equals(Value, assign),
            ActivationConditionOperator.NotEquals => !Equals(Value, assign),
            ActivationConditionOperator.GreaterThan => Compare(assign, Value) > 0,
            ActivationConditionOperator.GreaterThanOrEqual => Compare(assign, Value) >= 0,
            ActivationConditionOperator.LessThan => Compare(assign, Value) < 0,
            ActivationConditionOperator.LessThanOrEqual => Compare(assign, Value) <= 0,
            ActivationConditionOperator.In => Value is IEnumerable<object> list && list.Contains(assign),
            ActivationConditionOperator.NotIn => Value is IEnumerable<object> notList && !notList.Contains(assign),
            ActivationConditionOperator.StartsWith => assign is string s1 && Value is string p1 && s1.StartsWith(p1),
            ActivationConditionOperator.EndsWith => assign is string s2 && Value is string p2 && s2.EndsWith(p2),
            ActivationConditionOperator.Matches => assign is string str && Value is string pattern && Regex.IsMatch(str, pattern),
            _ => throw new NotSupportedException($"Unsupported operator: {Operator}")
        };
    }

    private static int Compare(object a, object b)
    {
        if (a is IComparable comparableA && b is IComparable)
            return comparableA.CompareTo(b);
        throw new InvalidOperationException($"Cannot compare values: {a} and {b}");
    }

    public override string ToString() => $"{Operator} {Value}";
}