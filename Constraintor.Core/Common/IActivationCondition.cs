namespace Constraintor.Core.Common;

public interface IActivationCondition : IDeepCloneable<IActivationCondition>
{
    bool Matches(object? assign);
}